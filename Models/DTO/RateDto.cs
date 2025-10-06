namespace ABC.Models.DTO
{
    public class RateDto
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public decimal Rate { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
