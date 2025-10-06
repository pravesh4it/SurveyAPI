namespace ABC.Models.DTO
{
    public class EmailAttachment
    {
        public string FileName { get; set; } = "";
        public string ContentType { get; set; } = "application/octet-stream";
        public byte[] Content { get; set; } = Array.Empty<byte>();
    }
}
