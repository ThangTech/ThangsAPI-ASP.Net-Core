namespace ThangAPI.Models.Domain
{
    public class Walkcs
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageURL { get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionID { get; set; }
        // Thuộc tính điều hướng

        public Difficulty Difficulty  { get; set; }
        public Region Region { get; set; }

    }
}
