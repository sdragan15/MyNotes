using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Application.Model;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyNotes.ViewModel
{
    public partial class NotesCreateViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCreate))]
        private string? body;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCreate))]
        private string? headTitle;
        private Guid originalId = Guid.Empty;

        private string? _draftBody;
        private string? _draftHeadTitle;
        private bool _suppressDraftSave;

        public bool CanCreate => !string.IsNullOrWhiteSpace(Body) && !string.IsNullOrWhiteSpace(HeadTitle);
        public bool IsEditMode => originalId != Guid.Empty;

        public NotesCreateViewModel() { }

        [RelayCommand]
        private async Task CreateAsync()
        {
            var opId = Guid.NewGuid().ToString("N");
            _suppressDraftSave = true;

            await Shell.Current.GoToAsync("..", new Dictionary<string, object>
            {
                ["action"] = originalId == Guid.Empty ? "create" : "update",
                ["id"] = originalId.ToString(),
                ["opId"] = opId,
                ["createdText"] = Body?.Trim() ?? string.Empty,
                ["headTitle"] = HeadTitle?.Trim() ?? string.Empty
            });

            if (originalId == Guid.Empty)
            {
                _draftBody = null;
                _draftHeadTitle = null;
            }

            _suppressDraftSave = false;
            Reset();
        }

        [RelayCommand]
        private Task CancelAsync() => Shell.Current.GoToAsync("..");

        // Called from page's OnDisappearing — covers back arrow, cancel, and any other exit
        public void OnDisappearing()
        {
            if (_suppressDraftSave || originalId != Guid.Empty) return;

            _draftBody = Body;
            _draftHeadTitle = HeadTitle;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            bool hasItem = query.TryGetValue("item", out var itemObj);

            if (!hasItem)
            {
                RestoreOrReset();
                return;
            }

            var originalItem = JsonSerializer.Deserialize<NotesDto>(itemObj?.ToString() ?? string.Empty);
            if (originalItem == null) return;

            PopulateFrom(originalItem);
        }

        public void PopulateFrom(NotesDto originalItem)
        {
            originalId = originalItem.Id;
            HeadTitle = originalItem.Header;
            Body = originalItem.Content;
        }

        private void RestoreOrReset()
        {
            originalId = Guid.Empty;
            HeadTitle = _draftHeadTitle ?? "";
            Body = _draftBody ?? "";
        }

        private void Reset()
        {
            originalId = Guid.Empty;
            HeadTitle = "";
            Body = "";
        }
    }
}
