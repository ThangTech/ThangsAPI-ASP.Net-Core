using System.ComponentModel.DataAnnotations;

namespace ThangAPI.Models.DTO
{
    public class UpdateWalkDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Kilometer over 50!!!")]
        public double LengthInKm { get; set; }
        public string? WalkImageURL { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionID { get; set; }
    }
}
