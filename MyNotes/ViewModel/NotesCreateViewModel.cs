using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Application.Model;
using MyNotes.Application.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MyNotes.ViewModel
{
    public partial class NotesCreateViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly CategoryService _categoryService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCreate))]
        private string? body;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCreate))]
        private string? headTitle;

        [ObservableProperty]
        private ObservableCollection<CategoryDto> categories = [];

        [ObservableProperty]
        private CategoryDto? selectedCategory;

        private Guid originalId = Guid.Empty;

        private string? _draftBody;
        private string? _draftHeadTitle;
        private CategoryDto? _draftCategory;
        private bool _suppressDraftSave;

        public bool CanCreate => !string.IsNullOrWhiteSpace(Body) && !string.IsNullOrWhiteSpace(HeadTitle);
        public bool IsEditMode => originalId != Guid.Empty;

        public NotesCreateViewModel(CategoryService categoryService)
        {
            _categoryService = categoryService;
            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var list = await _categoryService.GetAllAsync();
            Categories = new ObservableCollection<CategoryDto>(list);
            if (SelectedCategory == null)
                SelectedCategory = Categories.FirstOrDefault();
        }

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
                ["headTitle"] = HeadTitle?.Trim() ?? string.Empty,
                ["categoryId"] = SelectedCategory?.Id.ToString() ?? string.Empty,
                ["categoryName"] = SelectedCategory?.Name ?? string.Empty
            });

            if (originalId == Guid.Empty)
            {
                _draftBody = null;
                _draftHeadTitle = null;
                _draftCategory = null;
            }

            _suppressDraftSave = false;
            Reset();
        }

        [RelayCommand]
        private Task CancelAsync() => Shell.Current.GoToAsync("..");

        public void OnDisappearing()
        {
            if (_suppressDraftSave || originalId != Guid.Empty) return;

            _draftBody = Body;
            _draftHeadTitle = HeadTitle;
            _draftCategory = SelectedCategory;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            bool hasItem = query.TryGetValue("item", out var itemObj);

            if (!hasItem)
            {
                int? defaultCategoryId = null;
                if (query.TryGetValue("defaultCategoryId", out var defObj) && int.TryParse(defObj?.ToString(), out var defId))
                    defaultCategoryId = defId;

                RestoreOrReset(defaultCategoryId);
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
            SelectedCategory = originalItem.CategoryId.HasValue
                ? Categories.FirstOrDefault(c => c.Id == originalItem.CategoryId.Value)
                  ?? Categories.FirstOrDefault()
                : Categories.FirstOrDefault();
        }

        private void RestoreOrReset(int? defaultCategoryId = null)
        {
            originalId = Guid.Empty;
            HeadTitle = _draftHeadTitle ?? "";
            Body = _draftBody ?? "";

            if (_draftCategory != null)
                SelectedCategory = _draftCategory;
            else if (defaultCategoryId.HasValue)
                SelectedCategory = Categories.FirstOrDefault(c => c.Id == defaultCategoryId.Value) ?? Categories.FirstOrDefault();
            else
                SelectedCategory = Categories.FirstOrDefault();
        }

        private void Reset()
        {
            originalId = Guid.Empty;
            HeadTitle = "";
            Body = "";
            SelectedCategory = Categories.FirstOrDefault();
        }
    }
}
