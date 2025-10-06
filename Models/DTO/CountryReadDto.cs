namespace ABC.Models.DTO
{
    public record CountryReadDto(
        Guid Id,
        string Name,
        string Currency,
        string CurrencySymbol,
        string IsdCode,
        string ShortCode
    );
}
