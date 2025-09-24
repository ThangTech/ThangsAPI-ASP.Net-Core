using Microsoft.EntityFrameworkCore;
using ThangAPI.Data;
using ThangAPI.Models.Domain;

namespace ThangAPI.Repositoty
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly ThangDbContext thangDbContext;

        public SQLWalkRepository(ThangDbContext thangDbContext)
        {
            this.thangDbContext = thangDbContext;
        }
        public async Task<Walkcs> CreateWalkAsync(Walkcs walkcs)
        {
            await thangDbContext.Walkcs.AddAsync(walkcs);
            await thangDbContext.SaveChangesAsync();
            return walkcs;
        }

        public async Task<Walkcs?> DeleteWalkAsync(Guid id)
        {
            var existWalk = await thangDbContext.Walkcs.FirstOrDefaultAsync(x => x.Id == id);
            if (existWalk == null)
            {
                return null;
            }
            thangDbContext.Remove(existWalk);
            await thangDbContext.SaveChangesAsync();
            return existWalk;

        }

        public async Task<List<Walkcs>> GetAllWalkAsync()
        {
            var walks = await thangDbContext.Walkcs.Include("Difficulty").Include("Region").ToListAsync(); // Navigation property
            return walks;
        }

        public async Task<Walkcs?> GetWalkIDAsync(Guid id)
        {
            var walkId = await thangDbContext.Walkcs.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            return walkId;
        }

        public async Task<Walkcs?> UpdateWalkAsync(Guid id, Walkcs walkcs)
        {
            var existWalk = await thangDbContext.Walkcs.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (existWalk == null)
            {
                return null;
            }
            existWalk.Name = walkcs.Name;
            existWalk.Description = walkcs.Description;
            existWalk.LengthInKm = walkcs.LengthInKm;
            existWalk.WalkImageURL = walkcs.WalkImageURL;
            existWalk.DifficultyId = walkcs.DifficultyId;
            existWalk.RegionID = walkcs.RegionID;
            await thangDbContext.SaveChangesAsync();
            return existWalk;
        }
    }
}
