using ABC.Models.Domain;

namespace ABC.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country> GetByIdAsync(Guid id);
        Task<Country> AddAsync(Country entity);
        Task<bool> UpdateAsync(Country entity);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
