using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MyNotes.Application.Model;
using MyNotes.Application.Services;
using MyNotes.Messages;
using MyNotes.View;
using System.Collections.ObjectModel;
using System.Text.Json;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace MyNotes.ViewModel
{
    public partial class NotesViewModel : BaseViewModel
    {
        public NotesService _notesService { get; set; }

        [ObservableProperty]
        private ObservableCollection<NotesGroup> noteItems = [];
        private string _lastOpId;

        private readonly List<NotesDto> _allNotes = [];
        private int? _filterCategoryId;
        private string? _filterCategoryName;

        [ObservableProperty]
        private string pageTitle = "Notes";

        [ObservableProperty]
        private bool selectionMode = false;
        [ObservableProperty]
        private int selectDuration = 500;

        public NotesViewModel(NotesService notesService)
        {
            _notesService = notesService;
            SelectionMode = false;
            SelectDuration = 500;

            WeakReferenceMessenger.Default.Register<CategoryFilterChangedMessage>(this, (_, m) =>
            {
                _filterCategoryId = m.CategoryId;
                _filterCategoryName = m.CategoryName;
                PageTitle = m.CategoryName ?? "Notes";
                RebuildGroups();
            });
        }

        private void RebuildGroups()
        {
            var filtered = _filterCategoryId.HasValue
                ? _allNotes.Where(n => n.CategoryId == _filterCategoryId)
                : _allNotes;

            var groups = filtered
                .GroupBy(n => n.LastUpdateTime.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new NotesGroup(g.Key, g.OrderByDescending(n => n.LastUpdateTime)))
                .ToList();

            NoteItems = new ObservableCollection<NotesGroup>(groups);
        }

        [RelayCommand]
        public async Task GetNotes()
        {
            var notes = await _notesService.GetAllNotesAsync();
            _allNotes.Clear();
            _allNotes.AddRange(notes);
            RebuildGroups();
        }

        [RelayCommand]
        public async Task EditItem(NotesDto item)
        {
            if (SelectionMode) return;

            var itemSerialized = JsonSerializer.Serialize(item);
            await Shell.Current.GoToAsync(nameof(NotesCreatePage), new Dictionary<string, object>
            {
                ["item"] = itemSerialized
            });
        }

        [RelayCommand]
        public async Task CreateItem()
        {
            var query = new Dictionary<string, object>();
            if (_filterCategoryId.HasValue)
                query["defaultCategoryId"] = _filterCategoryId.Value.ToString();

            await Shell.Current.GoToAsync(nameof(NotesCreatePage), query);
        }

        [RelayCommand]
        public async Task DeleteNote(NotesDto note)
        {
            if (note == null) return;

            var popup = new DeleteNotePopup(note.Header ?? string.Empty);
            var result = await MauiApp.Current!.Windows[0].Page!.ShowPopupAsync(popup);

            if (result is not true) return;

            _allNotes.Remove(note);
            RebuildGroups();
            await _notesService.DeleteNoteAsync(note.Id);
        }

        public async Task OnAppearing(IDictionary<string, object> query)
        {
            if (query == null || query.Count == 0) return;

            if (!query.TryGetValue("action", out var actionObj)) return;

            var action = actionObj?.ToString() ?? "";
            if (action == "create")
                await CreateNotes(query);
            else if (action == "update")
                await UpdateNotes(query);
        }

        private async Task UpdateNotes(IDictionary<string, object> query)
        {
            bool hasId = query.TryGetValue("id", out var idObj);
            bool hasBody = query.TryGetValue("createdText", out var bodyObj);
            bool hasTitle = query.TryGetValue("headTitle", out var titleObj);
            query.TryGetValue("opId", out var opIdObj);

            if (!hasBody || !hasTitle || !hasId) return;

            var body = bodyObj?.ToString();
            var title = titleObj?.ToString();
            var opId = opIdObj!.ToString();

            if (string.IsNullOrWhiteSpace(body) || string.IsNullOrWhiteSpace(title)
                || !Guid.TryParse(idObj?.ToString(), out var id)) return;

            if (opId.Equals(_lastOpId)) return;

            var existingItem = _allNotes.FirstOrDefault(n => n.Id.Equals(id));
            if (existingItem == null) return;

            existingItem.Header = title;
            existingItem.Content = body.Trim();
            existingItem.LastUpdateTime = DateTime.Now;
            if (query.TryGetValue("categoryId", out var catIdObj) && int.TryParse(catIdObj?.ToString(), out var catId))
            {
                existingItem.CategoryId = catId;
                existingItem.CategoryName = query.TryGetValue("categoryName", out var catNameObj) ? catNameObj?.ToString() ?? string.Empty : string.Empty;
            }
            RebuildGroups();

            await _notesService.UpdateNoteAsync(existingItem);
            _lastOpId = opId;
        }

        private async Task CreateNotes(IDictionary<string, object> query)
        {
            bool hasBody = query.TryGetValue("createdText", out var bodyObj);
            bool hasTitle = query.TryGetValue("headTitle", out var titleObj);
            query.TryGetValue("opId", out var opIdObj);

            if (!hasBody || !hasTitle) return;

            var body = bodyObj?.ToString();
            var title = titleObj?.ToString();
            var opId = opIdObj!.ToString();

            if (string.IsNullOrWhiteSpace(body) || string.IsNullOrWhiteSpace(title)) return;
            if (opId.Equals(_lastOpId)) return;

            int? categoryId = null;
            string categoryName = string.Empty;
            if (query.TryGetValue("categoryId", out var catIdObj) && int.TryParse(catIdObj?.ToString(), out var catId))
            {
                categoryId = catId;
                categoryName = query.TryGetValue("categoryName", out var catNameObj) ? catNameObj?.ToString() ?? string.Empty : string.Empty;
            }

            var newNote = new NotesDto
            {
                Header = title,
                Content = body.Trim(),
                CategoryId = categoryId,
                CategoryName = categoryName,
                DateCreated = DateTime.Now,
                LastUpdateTime = DateTime.Now
            };

            _allNotes.Insert(0, newNote);
            RebuildGroups();

            try
            {
                await _notesService.AddNoteAsync(newNote);
            }
            catch (Exception e)
            {
                throw new ApplicationException("An error occurred while adding the note.", e);
            }

            _lastOpId = opId;
        }
    }
}
