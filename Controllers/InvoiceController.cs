using ABC.Models.DTO;
using ABC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System.Web.Helpers;

namespace ABC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IEmailService emailService;

        public InvoiceController(IInvoiceRepository invoiceRepo, IEmailService emailService)
        {
            this._invoiceRepo = invoiceRepo ?? throw new ArgumentNullException(nameof(invoiceRepo));
            this.emailService = emailService;
        }

        /// <summary>
        /// Create invoice with line items
        /// POST: /api/invoice
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                // return modelstate errors
                var firstError = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();

                return BadRequest(firstError);
            }
            try
            {
                // map request to repository DTO (InvoiceCreateDto)
                // InvoiceCreateDto is an application DTO used by your repo/service layer.
                // If you used the InvoiceService earlier, its InvoiceCreateDto shape is compatible.
                var repoDto = new InvoiceCreateDto
                {
                    SurveyId = TryParseGuid(request.SurveyId),
                    ClientSurveyName = request.ClientSurveyName,
                    PONumber = request.PONumber,
                    AccountEmail = request.AccountEmail,
                    AddrLine1 = request.Address?.Line1,
                    AddrLine2 = request.Address?.Line2,
                    AddrLine3 = request.Address?.Line3,
                    ZipCode = request.Address?.Zip,
                    Notes = null,
                    DueDate = null,
                    CurrencyCode = "INR",
                    Items = request.Rows?.Select(r => new InvoiceItemDto
                    {
                        Quantity = r.Quantity,
                        Description = r.Description,
                        UnitCost = r.Unit,
                        TaxAmount = r.TaxAmount ?? 0,
                        DiscountAmount = r.DiscountAmount ?? 0
                    }).ToList() ?? new System.Collections.Generic.List<InvoiceItemDto>()
                };

                // Basic server-side validation: ensure at least one valid item
                if (repoDto.Items == null || !repoDto.Items.Any())
                {
                    return BadRequest("At least one invoice item is required.");
                }

                // createdBy - get from authenticated user if available
                var createdBy = User?.Identity?.Name;

                // call repository to create invoice
                var (invoiceId, invoiceNumber) = await _invoiceRepo.CreateInvoiceAsync(repoDto, createdBy);

                // Return 201 Created with location header
                return CreatedAtAction(nameof(GetInvoice), new { id = invoiceId }, "Invoice Created");
            }
            catch (Exception ex)
            {
                // log error (use ILogger in real app)
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// GET invoice by id with line items
        /// GET: /api/invoice/{id}
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetInvoice([FromRoute] Guid id)
        {
            try
            {
                var invoice = await _invoiceRepo.GetInvoiceByIdAsync(id);
                if (invoice == null)
                {
                    return NotFound();
                }
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error in fetching data");
            }
        }
        // GET: /api/invoice/by-survey/{surveyId}
        [HttpGet("by-survey/{surveyId:guid}")]
        public async Task<IActionResult> GetBySurvey(Guid surveyId)
        {
            var invoices = await _invoiceRepo.GetInvoicesBySurveyIdAsync(surveyId); // implement this in repo
            return Ok(invoices);
        }

        // GET: /api/invoice/{invoiceId}/pdf
        [HttpGet("{invoiceId:guid}/pdf")]
        public async Task<IActionResult> GetInvoicePdf([FromRoute] Guid invoiceId)
        {
            try
            {


                // get invoice DTO for view model
                var invoiceDto = await _invoiceRepo.GetInvoiceDtoByIdAsync(invoiceId); // add method to repo to map to InvoiceMasterDto
                if (invoiceDto == null) return NotFound();

                // Use Rotativa to render view as PDF
                var pdf = new ViewAsPdf("Invoice/InvoicePdf", invoiceDto)
                {
                    // Optional options:
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Portrait,
                    FileName = $"{invoiceDto.InvoiceNumber}.pdf"
                };
                return pdf; // Rotativa returns FileResult

            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        // POST: /api/invoice/{invoiceId}/send
        [HttpPost("{invoiceId:guid}/send")]
        public async Task<IActionResult> SendInvoice([FromRoute] Guid invoiceId)
        {
            var invoiceDto = await _invoiceRepo.GetInvoiceDtoByIdAsync(invoiceId);
            if (invoiceDto == null) return NotFound();

            // Render to PDF byte[] using Rotativa
            var pdfResult = new ViewAsPdf("Invoice/InvoicePdf", invoiceDto)
            {
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait
            };

            // Rotativa's ViewAsPdf inherits ActionResult; to get bytes, use BuildFile() extension (internal). 
            // But Rotativa does not expose a direct asynchronous API to get bytes. We'll render to stream by invoking controller action.
            byte[] pdfBytes;
            using (var memory = new MemoryStream())
            {
                var file = await pdfResult.BuildFile(this.ControllerContext); // BuildFile returns byte[]
                pdfBytes = file;
            }

            // send email
            using (var ms = new MemoryStream(pdfBytes))
            {
                var subject = $"Invoice {invoiceDto.InvoiceNumber}";
                var body = $"Please find attached invoice {invoiceDto.InvoiceNumber}";

                // AccountEmail is target address
                await emailService.SendEmailWithAttachmentAsync(invoiceDto.AccountEmail, subject, body, ms, $"{invoiceDto.InvoiceNumber}.pdf");
            }

            return Ok("sent successfully");
        }

        // POST: /api/invoice/{id}/send-with-attachment
        [HttpPost("{id:guid}/send-with-attachment")]
        public async Task<IActionResult> SendWithAttachment([FromRoute] Guid id, [FromForm] SendInvoiceAttachmentRequest model)
        {
            if (model?.Pdf == null || model.Pdf.Length == 0)
                return BadRequest(new { message = "PDF file is required." });

            if (string.IsNullOrWhiteSpace(model.ToEmail))
                return BadRequest(new { message = "Recipient email (toEmail) is required." });

            using var ms = new MemoryStream();
            await model.Pdf.CopyToAsync(ms);
            var bytes = ms.ToArray();

            var fileName = string.IsNullOrWhiteSpace(model.Pdf.FileName) ? $"invoice-{id}.pdf" : model.Pdf.FileName;

            await emailService.SendEmailAsync(
                toEmails: model.ToEmail,
                subject: string.IsNullOrWhiteSpace(model.Subject) ? $"Invoice {id}" : model.Subject,
                htmlBody: string.IsNullOrWhiteSpace(model.Body) ? "Please find attached invoice." : model.Body,
                attachments: new List<EmailAttachment> {
                new EmailAttachment
                {
                    FileName = fileName,
                    ContentType = "application/pdf",
                    Content = bytes
                }
                });

            return Ok(new { message = "Email sent with attachment." });
        }

        // helper: try parse guid, else return null
        private Guid? TryParseGuid(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return Guid.TryParse(s, out var g) ? g : (Guid?)null;
        }
    }
}
