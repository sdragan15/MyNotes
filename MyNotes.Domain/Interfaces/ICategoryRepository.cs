using MyNotes.Domain.Entities;

namespace MyNotes.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<NoteCategory>> GetAllAsync();
    }
}
