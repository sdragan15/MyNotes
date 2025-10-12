using MyNotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Domain.Interfaces
{
    public interface INotesRepository
    {
        Task AddAsync(Notes item);
        Task UpdateAsync(Notes item);
        Task DeleteAsync(Notes item);
        Task<Notes?> GetByIdAsync(Guid id);
        Task<List<Notes>> GetAllAsync();
    }
}
