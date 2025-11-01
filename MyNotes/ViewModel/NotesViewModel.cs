using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Application.Model;
using MyNotes.Application.Services;
using MyNotes.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace MyNotes.ViewModel
{
    public partial class NotesViewModel : BaseViewModel
    {
        public NotesService _notesService { get; set; }

        [ObservableProperty]
        private ObservableCollection<NotesDto> noteItems = [];
        private string _lastOpId;

        public NotesViewModel(NotesService notesService)
        {
            _notesService = notesService;
        }

        [RelayCommand]
        public async Task GetNotes()
        {
            var notes = await _notesService.GetAllNotesAsync();
            NoteItems = new ObservableCollection<NotesDto>(notes.OrderByDescending(x => x.DateCreated));
        }



        [RelayCommand]
        public async Task EditItem(NotesDto item)
        {
            var itemSerialized = JsonSerializer.Serialize(item);

            await Shell.Current.GoToAsync(nameof(MyNotes.View.NotesCreatePage), new Dictionary<string, object>
            {
                ["item"] = itemSerialized
            });
        }

        [RelayCommand]
        public async Task CreateItem()
        {
            await Shell.Current.GoToAsync(nameof(MyNotes.View.NotesCreatePage));
        }

        [RelayCommand]
        public async Task SelectNote(NotesDto note)
        {
            note.IsSelected = true;
        }

        
        public async Task OnAppearing(IDictionary<string, object> query)
        {
            if(query == null || query.Count == 0)
            {
                return;
            }

            var action = "";
            bool hasAction = query.TryGetValue("action", out var actionObj);
            if (!hasAction)
            {
                return;
            }

            action = actionObj?.ToString() ?? "";
            if (action == "create")
            {
                await CreateNotes(query);
            }
            else if (action == "update")
            {
                await UpdateNotes(query);
            }
        }

        private async Task UpdateNotes(IDictionary<string, object> query)
        {
            Guid id = Guid.Empty;
            string? body = "";
            string? title = "";
            string opId;

            bool hasId = query.TryGetValue("id", out var idObj);
            bool hasBody = query.TryGetValue("createdText", out var bodyObj);
            bool hasTitle = query.TryGetValue("headTitle", out var titleObj);
            query.TryGetValue("opId", out var opIdObj);

            if (!hasBody || !hasTitle || !hasId)
            {
                return;
            }

            body = bodyObj?.ToString();
            title = titleObj?.ToString();
            opId = opIdObj!.ToString();

            if (string.IsNullOrWhiteSpace(body) || string.IsNullOrWhiteSpace(title) || !Guid.TryParse(idObj?.ToString(), out id))
            {
                return;
            }

            if (opId.Equals(_lastOpId))
            {
                return;
            }

            var existingItem = NoteItems.FirstOrDefault(n => n.Id.Equals(id));
            if(existingItem == null)
            {
                return;
            }

            NoteItems.Remove(existingItem);
            existingItem.Header = title;
            existingItem.Content = body.Trim();

            NoteItems.Insert(0, existingItem);
            await _notesService.UpdateNoteAsync(existingItem);

            _lastOpId = opId;
        }

        private async Task CreateNotes(IDictionary<string, object> query)
        {
            string? body = "";
            string? title = "";
            string opId;

            bool hasBody = query.TryGetValue("createdText", out var bodyObj);
            bool hasTitle = query.TryGetValue("headTitle", out var titleObj);
            query.TryGetValue("opId", out var opIdObj);

            if (!hasBody || !hasTitle)
            {
                return;
            }

            body = bodyObj?.ToString();
            title = titleObj?.ToString();
            opId = opIdObj!.ToString();

            if (string.IsNullOrWhiteSpace(body) || string.IsNullOrWhiteSpace(title))
            {
                return;
            }

            if (opId.Equals(_lastOpId))
            {
                return;
            }

            var newNotes = new NotesDto()
            {
                Header = title,
                Content = body.Trim(),
                DateCreated = DateTime.Now
            };

            NoteItems.Insert(0, newNotes);

            try
            {
                await _notesService.AddNoteAsync(newNotes);
            }
            catch(Exception e)
            {
                throw new ApplicationException("An error occurred while retrieving items.", e);
            }
            

            _lastOpId = opId;
        }
    }
}
