namespace ABC.Models.Domain
{
    public class VendorAllocationEmailAttachment
    {
        public Guid Id { get; set; }

        public Guid VendorAllocationEmailHistoryId { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public bool IsSystemGenerated { get; set; }

        public virtual VendorAllocationEmailHistory VendorAllocationEmailHistory { get; set; }
    }
}
