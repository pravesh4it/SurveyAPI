using ABC.Models.Domain;

namespace ABC.Repositories
{
    public interface IRateRepository
    {
        Task<RateHistory> GetActiveRateAsync(string entityType, Guid entityId, DateTime asOfDate);
        Task<IEnumerable<RateHistory>> GetHistoryAsync(string entityType, Guid entityId);
        Task<RateHistory> GetRateAsOfAsync(string entityType, Guid entityId, DateTime date);
        Task AddAsync(RateHistory rate);
        Task UpdateAsync(RateHistory rate);
        Task SaveChangesAsync();
    }
}
