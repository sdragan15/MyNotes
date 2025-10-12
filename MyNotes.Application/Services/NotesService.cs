using MyNotes.Application.Model;
using MyNotes.Domain.Entities;
using MyNotes.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    DateCreated = DateTime.UtcNow
                };
                await _notesRepository.AddAsync(itemEntity);
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.Message;
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
                {
                    await _notesRepository.DeleteAsync(existingNote);
                }
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
                var notes = await _notesRepository.GetAllAsync();
                var notesDto = notes.Select(item => new NotesDto
                {
                    Id = item.Id,
                    Header = item.Title,
                    Content = item.Body,
                    DateCreated = item.DateCreated
                }).ToList();

                return notesDto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving notes.", ex);
            }

        }
    }
}
