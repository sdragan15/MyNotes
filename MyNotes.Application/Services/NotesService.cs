using MyNotes.Application.Model;
using MyNotes.Domain.Entities;
using MyNotes.Domain.Interfaces;

namespace MyNotes.Application.Services
{
    public class NotesService
    {
        public INotesRepository _notesRepository;

        public NotesService(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public async Task AddNoteAsync(NotesDto item)
        {
            try
            {
                var itemEntity = new Notes
                {
                    Title = item.Header,
                    Body = item.Content,
                    CategoryId = item.CategoryId,
                    DateCreated = DateTime.UtcNow,
                    LastUpdateTime = DateTime.UtcNow
                };
                await _notesRepository.AddAsync(itemEntity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding the note.", ex);
            }
        }

        public async Task UpdateNoteAsync(NotesDto item)
        {
            try
            {
                var existingNote = await _notesRepository.GetByIdAsync(item.Id);
                if (existingNote != null)
                {
                    existingNote.Title = item.Header;
                    existingNote.Body = item.Content;
                    existingNote.CategoryId = item.CategoryId;
                    existingNote.LastUpdateTime = DateTime.UtcNow;
                    await _notesRepository.UpdateAsync(existingNote);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the note.", ex);
            }
        }

        public async Task DeleteNoteAsync(Guid itemId)
        {
            try
            {
                var existingNote = await _notesRepository.GetByIdAsync(itemId);
                if (existingNote != null)
                    await _notesRepository.DeleteAsync(existingNote);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the note.", ex);
            }
        }

        public async Task<List<NotesDto>> GetAllNotesAsync()
        {
            try
            {
                var notes = await _notesRepository.GetAllWithCategoryAsync();
                return notes.Select(item => new NotesDto
                {
                    Id = item.Id,
                    Header = item.Title,
                    Content = item.Body,
                    CategoryId = item.CategoryId,
                    CategoryName = item.Category?.Name ?? string.Empty,
                    DateCreated = item.DateCreated,
                    LastUpdateTime = item.LastUpdateTime == default ? item.DateCreated : item.LastUpdateTime
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving notes.", ex);
            }
        }
    }
}
