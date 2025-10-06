using ABC.Models.Domain;

namespace ABC.Repositories
{
    public interface IMultiselectRepository
    {
        Task<IEnumerable<MultiSelect>> GetBySelectionTypeAsync(string selectionType);
        Task<MultiSelect?> GetByIdAsync(Guid id);
        Task<MultiSelect> AddAsync(MultiSelect entity);
        Task UpdateAsync(MultiSelect entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
