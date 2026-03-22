using DigMap.Application.Interfaces.Repositories;
using DigMap.Domain.Entities;
using DigMap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DigMap.Infrastructure.Repositories
{
    public class FindItemRepository : IFindItemRepository
    {
        private readonly AppDbContext _context;

        public FindItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FindItem>> GetAllByUserIdAsync(string userId)
        {
            return await _context.FindItems
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<FindItem?> GetByIdAsync(int id)
        {
            return await _context.FindItems.FindAsync(id);
        }

        public async Task AddAsync(FindItem item)
        {
            _context.FindItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FindItem item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FindItem item)
        {
            _context.FindItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}