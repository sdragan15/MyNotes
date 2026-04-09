using Microsoft.EntityFrameworkCore;
using MyNotes.Domain.Entities;
using MyNotes.Domain.Interfaces;
using MyNotes.Infrastructure.Sqlite.Data;

namespace MyNotes.Infrastucture.Sqlite.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TodoContext _context;

        public CategoryRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<List<NoteCategory>> GetAllAsync()
        {
            return await _context.NoteCategories.ToListAsync();
        }
    }
}
