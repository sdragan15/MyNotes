using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MyNotes.Application.Model;
using MyNotes.Application.Services;
using MyNotes.Messages;
using System.Collections.ObjectModel;

namespace MyNotes.ViewModel
{
    public partial class AppShellViewModel : ObservableObject
    {
        private readonly CategoryService _categoryService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ChevronRotation))]
        private bool isCategoriesExpanded;

        public double ChevronRotation => IsCategoriesExpanded ? 90.0 : 0.0;

        [ObservableProperty]
        private ObservableCollection<CategoryDto> categories = [];

        [ObservableProperty]
        private int? selectedCategoryId;

        public AppShellViewModel(CategoryService categoryService)
        {
            _categoryService = categoryService;
            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            var list = await _categoryService.GetAllAsync();
            Categories = new ObservableCollection<CategoryDto>(list);
        }

        [RelayCommand]
        public async Task NavigateToTodos()
        {
            await Shell.Current.GoToAsync("//MainPage");
            Shell.Current.FlyoutIsPresented = false;
        }

        [RelayCommand]
        public async Task NavigateToAllNotes()
        {
            SelectedCategoryId = null;
            WeakReferenceMessenger.Default.Send(new CategoryFilterChangedMessage(null, null));
            await Shell.Current.GoToAsync("//NotesPage");
            Shell.Current.FlyoutIsPresented = false;
        }

        [RelayCommand]
        public void ToggleCategoriesExpanded()
        {
            IsCategoriesExpanded = !IsCategoriesExpanded;
        }

        [RelayCommand]
        public void SelectCategory(CategoryDto category)
        {
            SelectedCategoryId = category.Id;
            WeakReferenceMessenger.Default.Send(new CategoryFilterChangedMessage(category.Id, category.Name));
            Shell.Current.GoToAsync("//NotesPage");
            Shell.Current.FlyoutIsPresented = false;
        }
    }
}
