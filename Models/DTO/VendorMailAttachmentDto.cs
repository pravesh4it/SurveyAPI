namespace ABC.Models.DTO
{
    public class VendorMailAttachmentDto
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public bool IsSystemGenerated { get; set; }
    }
}