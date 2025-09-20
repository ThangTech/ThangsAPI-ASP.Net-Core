using Microsoft.EntityFrameworkCore;
using ThangAPI.Models.Domain;

namespace ThangAPI.Data
{
    public class ThangDbContext:DbContext
    {
        public ThangDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walkcs> Walkcs { get; set; }
    }
}
