namespace ABC.Models.DTO
{
    public class SendInvoiceAttachmentRequest
    {
        public IFormFile Pdf { get; set; } // form key: "pdf"
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
