using ThangAPI.Models.Domain;

namespace ThangAPI.Repositoty
{
    public interface IWalkRepository
    {
        Task<Walkcs> CreateWalkAsync(Walkcs walkcs);
        Task<List<Walkcs>> GetAllWalkAsync();
        Task<Walkcs?>GetWalkIDAsync(Guid id);
        Task<Walkcs?> UpdateWalkAsync(Guid id, Walkcs walkcs);
        Task<Walkcs?>DeleteWalkAsync(Guid id);
    }
}
