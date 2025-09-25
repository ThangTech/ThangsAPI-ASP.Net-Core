using ThangAPI.Models.Domain;

namespace ThangAPI.Repositoty
{
    public interface IWalkRepository
    {
        Task<Walkcs> CreateWalkAsync(Walkcs walkcs);
        Task<List<Walkcs>> GetAllWalkAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
            bool isAscending = true,int pageNumber = 1, int pageSize = 1000);
        Task<Walkcs?>GetWalkIDAsync(Guid id);
        Task<Walkcs?> UpdateWalkAsync(Guid id, Walkcs walkcs);
        Task<Walkcs?>DeleteWalkAsync(Guid id);
    }
}
