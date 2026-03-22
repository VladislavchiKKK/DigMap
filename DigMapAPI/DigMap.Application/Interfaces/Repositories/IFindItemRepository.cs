using DigMap.Domain.Entities;

namespace DigMap.Application.Interfaces.Repositories
{
    public interface IFindItemRepository
    {
        Task<List<FindItem>> GetAllByUserIdAsync(string userId);

        Task<FindItem?> GetByIdAsync(int id);

        Task AddAsync(FindItem item);

        Task UpdateAsync(FindItem item);

        Task DeleteAsync(FindItem item);
    }
}