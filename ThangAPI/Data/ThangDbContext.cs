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

        protected override void OnModelCreating(ModelBuilder modelBuilder) // override + OnModelCreating => Seed data
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for difficultty
            var difficultites = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("bdff050c-6d1e-4693-ac1a-ee180519a28d"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("245e8287-3b85-41bb-9303-6c2f52ff26eb"),
                    Name = "Normal"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("360a04ba-90b0-4d45-90f3-77e89eb77f79"),
                    Name = "Hard"
                }
                
            };
            // Seed data
            modelBuilder.Entity<Difficulty>().HasData(difficultites);

            // Seed data from Region
            var regions = new List<Region>() {
                new Region()
                {
                    Id = Guid.Parse("4c4044e0-59da-4070-b4be-4ee6c7dcefed"),
                    Name = "London",
                    Code = "LON",
                    RegionImageURL = "london.jpg"
                },
                new Region()
                {
                    Id = Guid.Parse("242c94d0-f607-4c35-acd8-b5d48394d8de"),
                    Name = "Manchester",
                    Code = "MAN",
                    RegionImageURL = "manchester.jpg"
                },
                new Region()
                {
                    Id = Guid.Parse("d154e851-3c7f-4e9f-b6ff-1030701ec74f"),
                    Name = "Berlin",
                    Code = "BER",
                    RegionImageURL = "berlin.jpg"
                },
                new Region()
                {
                    Id = Guid.Parse("2b36212b-ce30-46e5-bc7d-9aae18decdc2"),
                    Name = "Munich",
                    Code = "MUN",
                    RegionImageURL = "munich.jpg"
                },
                new Region()
                {
                    Id = Guid.Parse("8779c4b0-b32d-4b32-8cd7-1a219467a256"),
                    Name = "Ha noiu",
                    Code = "HAN",
                    RegionImageURL = "hanoi.jpg"
                }
            };
            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}
