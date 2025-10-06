using ABC.Data;
using ABC.Models.Domain;
using ABC.Models.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ABC.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AbcDbContext _db;
        public InvoiceRepository(AbcDbContext db)
        {
            _db = db;
        }
        public async Task<(Guid invoiceId, string invoiceNumber)> CreateInvoiceAsync(InvoiceCreateDto dto, string? createdBy = null)
        {
            using var tx = await _db.Database.BeginTransactionAsync();

            try
            {
                var invoice = new InvoiceMaster
                {
                    InvoiceId = Guid.NewGuid(),
                    InvoiceNumber = GenerateInvoiceNumber(),
                    SurveyId = dto.SurveyId,
                    ClientSurveyName = dto.ClientSurveyName,
                    PONumber = dto.PONumber,
                    AccountEmail = dto.AccountEmail,
                    AddrLine1 = dto.AddrLine1,
                    AddrLine2 = dto.AddrLine2,
                    AddrLine3 = dto.AddrLine3,
                    ZipCode = dto.ZipCode,
                    InvoiceDate = DateTime.UtcNow,
                    DueDate = dto.DueDate,
                    CurrencyCode = dto.CurrencyCode,
                    Notes = dto.Notes,
                    CreatedBy = createdBy,
                    CreatedAt = DateTime.UtcNow
                };

                _db.InvoiceMasters.Add(invoice);

                var lineNo = 0;
                decimal grandTotal = 0m;
                decimal taxTotal = 0m;
                decimal discountTotal = 0m;

                foreach (var it in dto.Items)
                {
                    lineNo++;
                    var lineTotal = Math.Round(it.Quantity * it.UnitCost, 2);
                    grandTotal += lineTotal;
                    taxTotal += it.TaxAmount;
                    discountTotal += it.DiscountAmount;

                    var txEntity = new InvoiceTransaction
                    {
                        InvoiceTxId = Guid.NewGuid(),
                        InvoiceId = invoice.InvoiceId,
                        LineNo = lineNo,
                        Quantity = it.Quantity,
                        Description = it.Description,
                        UnitCost = it.UnitCost,
                        LineTotal = lineTotal,
                        TaxAmount = it.TaxAmount,
                        DiscountAmount = it.DiscountAmount,
                        CreatedBy = createdBy,
                        CreatedAt = DateTime.UtcNow
                    };

                    _db.InvoiceTransactions.Add(txEntity);
                }

                invoice.GrandTotal = Math.Round(grandTotal, 2);
                invoice.TaxTotal = Math.Round(taxTotal, 2);
                invoice.DiscountTotal = Math.Round(discountTotal, 2);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return (invoice.InvoiceId, invoice.InvoiceNumber);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
        /// <summary>
        /// Fetch invoice with transactions by id. You might want to map to a DTO for API.
        /// </summary>
        public async Task<InvoiceMaster?> GetInvoiceByIdAsync(Guid invoiceId)
        {
            return await _db.InvoiceMasters
                .AsNoTracking()
                .Include(m => m.Transactions.OrderBy(t => t.LineNo))
                .FirstOrDefaultAsync(m => m.InvoiceId == invoiceId);
        }
        // returns list of invoice summary DTOs
        public async Task<List<InvoiceMasterDto>> GetInvoicesBySurveyIdAsync(Guid surveyId)
        {
            var invoices = await _db.InvoiceMasters
                .AsNoTracking()
                .Include(i => i.Transactions)
                .Where(i => i.SurveyId == surveyId)
                .ToListAsync();

            return invoices.Select(invoice => new InvoiceMasterDto
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                SurveyId = invoice.SurveyId,
                ClientSurveyName = invoice.ClientSurveyName,
                AccountEmail = invoice.AccountEmail,
                AddrLine1 = invoice.AddrLine1,
                AddrLine2 = invoice.AddrLine2,
                AddrLine3 = invoice.AddrLine3,
                ZipCode = invoice.ZipCode,
                InvoiceDate = invoice.InvoiceDate,
                GrandTotal = invoice.GrandTotal,
                TaxTotal = invoice.TaxTotal,
                DiscountTotal = invoice.DiscountTotal,
                PONumber= invoice.PONumber,
                Items = (invoice.Transactions ?? Enumerable.Empty<InvoiceTransaction>())
                            .OrderBy(t => t.LineNo)
                            .Select(t => new InvoiceItemDto
                            {
                                LineNo = t.LineNo,
                                Quantity = t.Quantity,
                                Description = t.Description,
                                UnitCost = t.UnitCost,
                                LineTotal = t.LineTotal,
                                TaxAmount = t.TaxAmount,
                                DiscountAmount = t.DiscountAmount
                            })
                            .ToList()
            }).ToList();
        }


        // returns InvoiceMasterDto fully populated for view
        public async Task<InvoiceMasterDto?> GetInvoiceDtoByIdAsync(Guid invoiceId)
        {
            var invoice = await _db.InvoiceMasters
                .AsNoTracking()
                .Include(i => i.Transactions)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);

            if (invoice == null) return null;

            return new InvoiceMasterDto
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNumber = invoice.InvoiceNumber,
                SurveyId = invoice.SurveyId,
                ClientSurveyName = invoice.ClientSurveyName,
                AccountEmail = invoice.AccountEmail,
                AddrLine1 = invoice.AddrLine1,
                AddrLine2 = invoice.AddrLine2,
                AddrLine3 = invoice.AddrLine3,
                ZipCode = invoice.ZipCode,
                InvoiceDate = invoice.InvoiceDate,
                GrandTotal = invoice.GrandTotal,
                TaxTotal = invoice.TaxTotal,
                DiscountTotal = invoice.DiscountTotal,
                Items = invoice.Transactions.OrderBy(t => t.LineNo).Select(t => new InvoiceItemDto
                {
                    LineNo = t.LineNo,
                    Quantity = t.Quantity,
                    Description = t.Description,
                    UnitCost = t.UnitCost,
                    LineTotal = t.LineTotal,
                    TaxAmount = t.TaxAmount,
                    DiscountAmount = t.DiscountAmount
                }).ToList()
            };
        }

        // Simple invoice number generator (human-readable)
        private string GenerateInvoiceNumber()
        {
            var stamp = DateTime.UtcNow.ToString("yyMMddHHmm"); // e.g. 2510041912
            var rnd = Math.Abs(Guid.NewGuid().GetHashCode()) % 10000;
            return $"PDR{stamp}-{rnd:D4}";
        }

    }
}
