using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThangAPI.Models.Domain;
using ThangAPI.Models.DTO;
using ThangAPI.Repositoty;

namespace ThangAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        //Post /api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageDTO imageDTO)
        {
            ValidateFileUpLoad(imageDTO);
            if (ModelState.IsValid)
            {
                //Convert DTO to domain
                var imageDomain = new Image
                {
                    File = imageDTO.File,
                    FileExtension = Path.GetExtension(imageDTO.File.FileName),
                    FileSizeInBytes = imageDTO.File.Length,
                    FileName = Path.GetFileNameWithoutExtension(imageDTO.File.FileName),
                    FileDescription = imageDTO.FileDescription
                };
                //User repository upload image
                await imageRepository.Upload(imageDomain);
                return Ok(imageDomain);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUpLoad(ImageDTO imageDTO)
        {
            var allowExtension = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowExtension.Contains(Path.GetExtension(imageDTO.File.FileName))) //Kiểm tra tệp mở rộng có dc phép hay ko
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }
            if(imageDTO.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB.");
            }
        }
    }
}
