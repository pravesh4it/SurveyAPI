namespace ABC.Models.DTO
{
    public class InvoiceDetailsDto
    {
        // If invoice exists, this will be non-null
        public InvoiceMasterDto? Invoice { get; set; }

        // If invoice does not exist, SurveyBasic will be filled (non-null)
        public SurveyBasicDto? SurveyBasic { get; set; }
    }

    public class InvoiceMasterDto
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public Guid? SurveyId { get; set; }
        public string? ClientSurveyName { get; set; }
        public string? PONumber { get; set; }
        public string AccountEmail { get; set; } = string.Empty;
        public string? AddrLine1 { get; set; }
        public string? AddrLine2 { get; set; }
        public string? AddrLine3 { get; set; }
        public string? ZipCode { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal DiscountTotal { get; set; }

        public List<InvoiceItemDto> Items { get; set; } = new();
    }

    public class SurveyBasicDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? Language { get; set; }
        public string? Country { get; set; }
        public int? Completes { get; set; }
        public decimal? Incidence { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateOnly LaunchedDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal ClientIR { get; set; }
        public decimal ClientRate { get; set; }
        public string? CurrencyId { get; set; }
    }
}
