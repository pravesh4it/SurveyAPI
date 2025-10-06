using ABC.Data;
using ABC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ABC.Repositories
{
    public class MultiselectRepository : IMultiselectRepository
    {
        private readonly AbcDbContext dbContext;

        public MultiselectRepository(AbcDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<MultiSelect>> GetBySelectionTypeAsync(string selectionType)
        => await dbContext.MultiSelects
        .AsNoTracking()
        .Where(x => x.SelectionType == selectionType)
        .OrderBy(x => x.DisplayName)
        .ToListAsync();


        public async Task<MultiSelect?> GetByIdAsync(Guid id)
        => await dbContext.MultiSelects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);


        public async Task<MultiSelect> AddAsync(MultiSelect entity)
        {
            entity.Id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
            dbContext.MultiSelects.Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }


        public async Task UpdateAsync(MultiSelect entity)
        {
            dbContext.MultiSelects.Update(entity);
            await dbContext.SaveChangesAsync();
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await dbContext.MultiSelects.FindAsync(id);
            if (existing is null) return false;
            dbContext.MultiSelects.Remove(existing);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
