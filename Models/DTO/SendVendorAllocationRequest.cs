namespace ABC.Models.DTO
{
    public class SendVendorAllocationRequest
    {
        public Guid SurveyId { get; set; }
        public Guid VendorId { get; set; }

        public string? VendorName { get; set; }

        public string PONumber { get; set; }

        public string Emails { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
        public List<IFormFile> Files { get; set; } = new();
        public string? SystemAttachmentName { get; set; }

    }

}
