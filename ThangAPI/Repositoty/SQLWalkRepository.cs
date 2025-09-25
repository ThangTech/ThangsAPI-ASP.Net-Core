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

        public async Task<List<Walkcs>> GetAllWalkAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = thangDbContext.Walkcs.Include("Difficulty").Include("Region").AsQueryable();
            //var walks = await thangDbContext.Walkcs.Include("Difficulty").Include("Region").ToListAsync(); // Navigation property
            // Filtering: lọc dữ liệu
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                // StringComparison.OrdinalIgnoreCase hỗn hợp chữ kể cả hoa hay thường
                // Viết nhiều else if để lọc theo cột
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }
            //Sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name): walks.OrderByDescending(x => x.Name);
                }
                if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm): walks.OrderByDescending(x => x.LengthInKm);
                }
            }
            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
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
