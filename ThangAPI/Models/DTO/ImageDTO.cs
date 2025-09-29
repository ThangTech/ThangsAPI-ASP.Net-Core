using System.ComponentModel.DataAnnotations;

namespace ThangAPI.Models.DTO
{
    public class ImageDTO
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; }
        public string? FileDescription { get; set; }
    }
}
