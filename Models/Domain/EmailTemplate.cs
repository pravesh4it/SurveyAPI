namespace ABC.Models.Domain
{
    public class EmailTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string DefaultMessage { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Is_Active { get; set; }

    }
}
