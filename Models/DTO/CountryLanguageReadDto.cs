namespace ABC.Models.DTO
{
    public record CountryLanguageReadDto(
        Guid Id,
        Guid CountryId,
        Guid LanguageId,
        string LanguageName,
        string LanguageDisplayName,
        bool IsPrimary
    );
}
