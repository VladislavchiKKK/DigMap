using DigMap.Application.DTOs.Finds;
using DigMap.Application.Interfaces.Repositories;
using DigMap.Application.Interfaces.Services;
using DigMap.Domain.Entities;

namespace DigMap.Application.Services
{
    public class FindItemService : IFindItemService
    {
        private readonly IFindItemRepository _repository;

        public FindItemService(IFindItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FindItemDto>> GetAllUserItemsAsync(string userId)
        {
            var items = await _repository.GetAllByUserIdAsync(userId);
            return items.Select(MapToDto).ToList();
        }

        public async Task<FindItemDto?> GetItemByIdAsync(int id, string userId)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null || item.UserId != userId) return null;

            return MapToDto(item);
        }

        public async Task<FindItemDto> CreateCoinAsync(CreateCoinDto dto, string userId)
        {
            var coin = new Coin
            {
                Name = dto.Name,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                DateFound = dto.DateFound,
                Year = dto.Year,
                Metal = dto.Metal,
                Denomination = dto.Denomination,
                UserId = userId
            };
            await _repository.AddAsync(coin);
            return MapToDto(coin);
        }

        public async Task<FindItemDto> CreateArtifactAsync(CreateArtifactDto dto, string userId)
        {
            var artifact = new Artifact
            {
                Name = dto.Name,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                DateFound = dto.DateFound,
                Era = dto.Era,
                Material = dto.Material,
                Class = dto.Class,
                UserId = userId
            };

            await _repository.AddAsync(artifact);

            return MapToDto(artifact);
        }

        public async Task DeleteItemAsync(int id, string userId)
        {
            var item = await _repository.GetByIdAsync(id);

            if (item == null || item.UserId != userId)
            {
                throw new UnauthorizedAccessException("Ви не маєте доступу до цього запису або його не існує.");
            }

            await _repository.DeleteAsync(item);
        }

        private FindItemDto MapToDto(FindItem item)
        {
            return item switch
            {
                Coin coin => new CoinDto
                {
                    Id = coin.Id,
                    Name = coin.Name,
                    Description = coin.Description,
                    Latitude = coin.Latitude,
                    Longitude = coin.Longitude,
                    DateFound = coin.DateFound,
                    Year = coin.Year,
                    Metal = coin.Metal,
                    Denomination = coin.Denomination
                },
                Artifact artifact => new ArtifactDto
                {
                    Id = artifact.Id,
                    Name = artifact.Name,
                    Description = artifact.Description,
                    Latitude = artifact.Latitude,
                    Longitude = artifact.Longitude,
                    DateFound = artifact.DateFound,
                    Era = artifact.Era,
                    Material = artifact.Material,
                    Class = artifact.Class
                },
                _ => throw new InvalidOperationException("Невідомий тип знахідки")
            };
        }
    }
}