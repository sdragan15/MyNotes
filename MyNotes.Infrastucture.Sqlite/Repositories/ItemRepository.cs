using Microsoft.EntityFrameworkCore;
using MyNotes.Domain.Entities;
using MyNotes.Domain.Interfaces;
using MyNotes.Infrastructure.Sqlite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Infrastucture.Sqlite.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly TodoContext _context;

        public ItemRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item item)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Item>> GetAllAsync()
        {
            var items = await _context.Items.ToListAsync();
            return items;
        }

        public async Task<List<Item>> GetAllCheckedAsync()
        {
            return await _context.Items.Where(x => x.IsChecked).ToListAsync();
        }

        public async Task<List<Item>> GetAllUncheckedAsync()
        {
            return await _context.Items.Where(x => !x.IsChecked).ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            return item;
        }

        public async Task UpdateAsync(Item item)
        {
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
