using ABC.Data;
using ABC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ABC.Repositories
{
    public class RateRepository : IRateRepository
    {
        private readonly AbcDbContext _context;

        public RateRepository(AbcDbContext context)
        {
            _context = context;
        }

        // Returns active rate for asOfDate (if null use today)
        public async Task<RateHistory> GetActiveRateAsync(string entityType, Guid entityId, DateTime asOfDate)
        {
            var d = asOfDate.Date;
            return await _context.RateHistory
                .Where(r => r.EntityType == entityType && r.EntityId == entityId
                            && (r.EndDate == null || r.EndDate >= d))
                .OrderByDescending(r => r.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RateHistory>> GetHistoryAsync(string entityType, Guid entityId)
        {
            return await _context.RateHistory
                .Where(r => r.EntityType == entityType && r.EntityId == entityId)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        public async Task<RateHistory> GetRateAsOfAsync(string entityType, Guid entityId, DateTime date)
        {
            var d = date.Date;
            return await _context.RateHistory
                .Where(r => r.EntityType == entityType && r.EntityId == entityId
                            && r.StartDate <= d
                            && (r.EndDate == null || r.EndDate >= d))
                .OrderByDescending(r => r.StartDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(RateHistory rate)
        {
            await _context.RateHistory.AddAsync(rate);
        }

        public Task UpdateAsync(RateHistory rate)
        {
            _context.RateHistory.Update(rate);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
