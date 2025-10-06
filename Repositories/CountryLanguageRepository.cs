using ABC.Data;
using ABC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ABC.Repositories
{
    public class CountryLanguageRepository : ICountryLanguageRepository
    {
        private readonly AbcDbContext dbContext;
        public CountryLanguageRepository(AbcDbContext dbContext) => this.dbContext = dbContext;

        public async Task<CountryLanguage> AddAsync(CountryLanguage entity)
        {
            await dbContext.CountryLanguages.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<CountryLanguage> entities)
        {
            await dbContext.CountryLanguages.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<CountryLanguage>> GetByCountryIdAsync(Guid countryId)
        {
            return await dbContext.CountryLanguages
                            .Where(cl => cl.CountryId == countryId)
                            .ToListAsync();
        }

        public async Task<IEnumerable<CountryLanguage>> GetByCountryIdWithLanguageAsync(Guid countryId)
        {
            return await dbContext.CountryLanguages
                            .Include(cl => cl.Language)
                            .Where(cl => cl.CountryId == countryId)
                            .ToListAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<CountryLanguage> entities)
        {
            dbContext.CountryLanguages.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid countryId, Guid languageId)
        {
            return await dbContext.CountryLanguages.AnyAsync(cl => cl.CountryId == countryId && cl.MultiSelectId == languageId);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
