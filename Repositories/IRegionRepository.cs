using ABC.Models.Domain;

namespace ABC.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
    }
}
