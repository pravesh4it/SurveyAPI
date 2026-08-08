namespace ABC.Models.DTO
{
    public class VendorMailHistoryDto
    {
        public Guid Id { get; set; }

        public DateTime SentDate { get; set; }

        public string Subject { get; set; }

        public string Emails { get; set; }

        public string Body { get; set; }

        public bool IsSent { get; set; }
        public List<VendorMailAttachmentDto> Attachments { get; set; } = new();
    }
}
