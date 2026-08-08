namespace ABC.Models.Domain
{
    public class VendorAllocationEmailHistory
    {
        public Guid Id { get; set; }

        public Guid SurveyId { get; set; }

        public Guid VendorId { get; set; }

        public string? VendorName { get; set; }

        public string PONumber { get; set; }

        public string Emails { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsSent { get; set; }

        public DateTime? SentDate { get; set; }

        public string? ErrorMessage { get; set; }

        public DateTime CreatedDate { get; set; }

    }

}
