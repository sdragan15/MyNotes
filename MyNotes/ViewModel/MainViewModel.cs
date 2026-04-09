using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Application.Model;
using MyNotes.Application.Services;
using MyNotes.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace MyNotes.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly ItemService _itemService;
        private readonly List<ItemDto> _allItems = [];

        [ObservableProperty] private ObservableCollection<TodoGroup> todoGroups = [];
        [ObservableProperty] private ObservableCollection<TodoGroup> doneGroups = [];
        [ObservableProperty] private bool isSidebarExpanded = true;
        [ObservableProperty] private string selectedSection = "Todos";

        public double SidebarWidth => IsSidebarExpanded ? 200 : 60;

        partial void OnIsSidebarExpandedChanged(bool value) => OnPropertyChanged(nameof(SidebarWidth));

        public MainViewModel(ItemService itemService)
        {
            Title = "Todo list";
            _itemService = itemService;
        }

        private void RebuildTodoGroups()
        {
            var groups = _allItems
                .Where(i => !i.IsChecked)
                .GroupBy(i => i.LastUpdateTime.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new TodoGroup(g.Key, g.OrderByDescending(i => i.LastUpdateTime)))
                .ToList();
            TodoGroups = new ObservableCollection<TodoGroup>(groups);
        }

        private void RebuildDoneGroups()
        {
            var groups = _allItems
                .Where(i => i.IsChecked)
                .GroupBy(i => i.LastUpdateTime.Date)
                .OrderByDescending(g => g.Key)
                .Select(g => new TodoGroup(g.Key, g.OrderByDescending(i => i.LastUpdateTime)))
                .ToList();
            DoneGroups = new ObservableCollection<TodoGroup>(groups);
        }

        [RelayCommand]
        public async Task GetItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            foreach (var item in items)
            {
                Attach(item);
                _allItems.Add(item);
            }
            RebuildTodoGroups();
            RebuildDoneGroups();
        }

        [RelayCommand]
        public async Task OpenPopup()
        {
            var vm = new ItemCreateViewModel { IsEdit = false };
            var result = (ItemEditResult?)await MauiApp.Current.MainPage.ShowPopupAsync(new ItemCreatePage(vm));
            if (result?.Action == ItemEditAction.Save && !string.IsNullOrWhiteSpace(result.Text))
            {
                var newItem = new ItemDto { Text = result.Text };
                Attach(newItem);
                _allItems.Insert(0, newItem);
                RebuildTodoGroups();
                await _itemService.AddItemAsync(newItem);
            }
        }

        [RelayCommand]
        public async Task EditItem(ItemDto item)
        {
            var vm = new ItemCreateViewModel { IsEdit = true, Text = item.Text };
            var result = (ItemEditResult?)await MauiApp.Current.MainPage.ShowPopupAsync(new ItemCreatePage(vm));
            if (result == null) return;

            if (result.Action == ItemEditAction.Save && !string.IsNullOrWhiteSpace(result.Text))
            {
                item.Text = result.Text;
                await _itemService.UpdateItemAsync(item);
            }
            else if (result.Action == ItemEditAction.Delete)
            {
                Detach(item);
                _allItems.Remove(item);
                RebuildTodoGroups();
                RebuildDoneGroups();
                await _itemService.DeleteItemAsync(item.Id);
            }
        }

        [RelayCommand]
        public void ToggleSidebar() => IsSidebarExpanded = !IsSidebarExpanded;
        [RelayCommand]
        public void ShowTodos() => SelectedSection = "Todos";
        [RelayCommand]
        public void ShowNotes() => SelectedSection = "Notes";

        private async Task CheckedChanged(ItemDto item)
        {
            if (item.IsChecked)
            {
                item.DateDone = DateTime.UtcNow;
                item.LastUpdateTime = DateTime.UtcNow;
                RebuildTodoGroups();
                RebuildDoneGroups();
                await _itemService.UpdateItemAsync(item);
            }
            else
            {
                item.DateDone = null;
                item.LastUpdateTime = DateTime.UtcNow;
                RebuildTodoGroups();
                RebuildDoneGroups();
                await _itemService.UpdateItemAsync(item);
            }
        }

        private void Attach(ItemDto item) => item.PropertyChanged += OnItemPropertyChanged;
        private void Detach(ItemDto item) => item.PropertyChanged -= OnItemPropertyChanged;

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not ItemDto item) return;
            if (e.PropertyName == nameof(ItemDto.IsChecked))
            {
                MainThread.BeginInvokeOnMainThread(async () => await CheckedChanged(item));
            }
        }
    }
}
