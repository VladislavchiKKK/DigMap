using DigMap.Application.DTOs.Finds;

namespace DigMap.Application.Interfaces.Services
{
    public interface IFindItemService
    {
        Task<List<FindItemDto>> GetAllUserItemsAsync(string userId);

        Task<FindItemDto?> GetItemByIdAsync(int id, string userId);

        Task<FindItemDto> CreateCoinAsync(CreateCoinDto dto, string userId);

        Task<FindItemDto> CreateArtifactAsync(CreateArtifactDto dto, string userId);

        Task DeleteItemAsync(int id, string userId);
    }
}