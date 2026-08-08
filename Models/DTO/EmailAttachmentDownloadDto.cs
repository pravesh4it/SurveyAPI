namespace ABC.Models.DTO
{
    public class EmailAttachmentDownloadDto
    {
        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}
