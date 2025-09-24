namespace ThangAPI.Models.DTO
{
    public class AddWalkDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageURL { get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionID { get; set; }
    }
}
