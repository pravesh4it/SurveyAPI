using ABC.Models.Domain;

namespace ABC.Repositories
{
    public interface ICountryLanguageRepository
    {
        Task<IEnumerable<CountryLanguage>> GetByCountryIdAsync(Guid countryId);
        Task<CountryLanguage> AddAsync(CountryLanguage entity);
        Task AddRangeAsync(IEnumerable<CountryLanguage> entities);
        Task RemoveRangeAsync(IEnumerable<CountryLanguage> entities);
        Task<IEnumerable<CountryLanguage>> GetByCountryIdWithLanguageAsync(Guid countryId);
        Task<bool> ExistsAsync(Guid countryId, Guid languageId);
        Task SaveChangesAsync();
    }
}
