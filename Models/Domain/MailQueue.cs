namespace ABC.Models.Domain
{
    public class MailQueue
    {
        public Guid Id { get; set; }
        public string ToMail { get; set; }
        public string FromMail { get; set; } = string.Empty;
        public string Subject { get; set; }
        public string content { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsSend { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}
