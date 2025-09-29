using ThangAPI.Data;
using ThangAPI.Models.Domain;

namespace ThangAPI.Repositoty
{
    public class LocalImageRepositoty : IImageRepository
    {
        private readonly IWebHostEnvironment webHost;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ThangDbContext thangDbContext;

        public LocalImageRepositoty(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor, ThangDbContext thangDbContext)
        {
            this.webHost = webHost;
            this.httpContextAccessor = httpContextAccessor;
            this.thangDbContext = thangDbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            //Tạo một biến đường dẫn trỏ đến thư mục cục bộ
            var localFilePath = Path.Combine(webHost.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

            //Upload Image đến đường dẫn cục bộ
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // http://localhost:1234/images/image.jpg

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://" +
                $"{httpContextAccessor.HttpContext.Request.Host}" +
                $"{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;
            //Add images to db
            await thangDbContext.Images.AddAsync(image);
            await thangDbContext.SaveChangesAsync();
            return image;
        }
    }
}
