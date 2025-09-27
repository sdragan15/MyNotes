using MyNotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Domain.Interfaces
{
    public interface IItemRepository
    {
        Task AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(Item item);
        Task<Item?> GetByIdAsync(Guid id);
        Task<List<Item>> GetAllAsync();
        Task<List<Item>> GetAllCheckedAsync();
        Task<List<Item>> GetAllUncheckedAsync();
    }
}
