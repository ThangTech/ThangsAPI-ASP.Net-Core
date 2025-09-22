
using Microsoft.EntityFrameworkCore;
using ThangAPI.Data;
using ThangAPI.Models.Domain;

namespace ThangAPI.Repositoty
{
    public class SQLRegionRepositorycs:IRegionRepository
    {
        private readonly ThangDbContext thangDbContext;

        public SQLRegionRepositorycs(ThangDbContext thangDbContext)
        {
            this.thangDbContext = thangDbContext;
        }

        public async Task<Region?> CreateAsync(Region region)
        {
           await thangDbContext.Regions.AddAsync(region);
           await thangDbContext.SaveChangesAsync();
           return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var existRegion = thangDbContext.Regions.FirstOrDefault(x => x.Id == id);  
            if (existRegion == null)
            {
                return null;
            }
            thangDbContext.Remove(existRegion);
            await thangDbContext.SaveChangesAsync();
            return existRegion;

        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await thangDbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIDAsync(Guid id)
        {
            return await thangDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existRegion = await thangDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existRegion == null)
            {
                return null;
            }
            existRegion.Code = region.Code;
            existRegion.Name = region.Name;
            existRegion.RegionImageURL = region.RegionImageURL;
            await thangDbContext.SaveChangesAsync();
            return existRegion;
        }
    }
}
