namespace ABC.Models.DTO
{
    public class CreateRateRequest
    {
        public decimal Rate { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; } // Expected as date (yyyy-MM-dd)
        public string Note { get; set; }
    }
}
