namespace ABC.Models.Domain
{
    public class RateHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // "Survey", "Partner", etc.
        public string EntityType { get; set; }

        // FK to Survey/Partner id
        public Guid EntityId { get; set; }

        // Numeric rate
        public decimal Rate { get; set; }

        // Currency code/name (choose what you prefer: ISO or id)
        public string Currency { get; set; }

        // Inclusive start date (business date)
        public DateTime StartDate { get; set; }

        // Inclusive end date, null means still active
        public DateTime? EndDate { get; set; }

        public string Note { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
