using System.ComponentModel.DataAnnotations;

namespace ThangAPI.Models.DTO
{
    public class AddRegionDTO
    {
        [Required] // Validation
        [MinLength(3, ErrorMessage = "Code have to minimum 3 of character")]
        [MaxLength(3, ErrorMessage = "Code have to maximum 3 of character")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? RegionImageURL { get; set; }
    }
}

