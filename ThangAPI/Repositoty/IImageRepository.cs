using ThangAPI.Models.Domain;

namespace ThangAPI.Repositoty
{
    public interface IImageRepository
    {
        Task<Image>Upload(Image image);
    }
}
