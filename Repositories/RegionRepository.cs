using ABC.Data;
using ABC.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ABC.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly AbcDbContext dbContext;

        public RegionRepository(AbcDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Region>> GetAllAsync()
        {
           return await dbContext.Regions.ToListAsync();
        }
    }
}
