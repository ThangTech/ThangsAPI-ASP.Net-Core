namespace ThangAPI.Models.DTO
{
    public class WalkDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageURL { get; set; }

        public  DifficultyDTO Difficulty { get; set; }
        public RegionDTO Region { get; set; }
    }
}
