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
    public class NotesRepository : INotesRepository
    {
        private readonly TodoContext _context;

        public NotesRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notes item)
        {
            _context.Notes.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notes item)
        {
            _context.Notes.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notes>> GetAllAsync()
        {
            var notes = await _context.Notes.ToListAsync();
            return notes;
        }

        public async Task<Notes?> GetByIdAsync(Guid id)
        {
            var notes = await _context.Notes.FindAsync(id);
            return notes;
        }

        public async Task UpdateAsync(Notes item)
        {
            _context.Notes.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
