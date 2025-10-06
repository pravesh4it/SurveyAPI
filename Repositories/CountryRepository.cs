using ABC.Data;
using ABC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ABC.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AbcDbContext _db;
        public CountryRepository(AbcDbContext db) => _db = db;

        public async Task<Country> AddAsync(Country entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            await _db.Countries.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _db.Countries.FindAsync(id);
            if (existing == null) return false;
            _db.Countries.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await _db.Countries.AsNoTracking().ToListAsync();
        }

        public async Task<Country> GetByIdAsync(Guid id)
        {
            return await _db.Countries.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Country entity)
        {
            var existing = await _db.Countries.FindAsync(entity.Id);
            if (existing == null) return false;

            // update allowed fields
            existing.Name = entity.Name;
            existing.Currency = entity.Currency;
            existing.CurrencySymbol = entity.CurrencySymbol;
            existing.IsdCode = entity.IsdCode;
            existing.ShortCode = entity.ShortCode;

            _db.Countries.Update(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _db.Countries.AnyAsync(c => c.Id == id);
        }
    }
}
