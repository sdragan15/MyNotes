using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Application.Model;
using MyNotes.Application.Services;
using MyNotes.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace MyNotes.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        ItemService _itemService;
        public ObservableCollection<ItemDto> Items { get; } = [];
        public ObservableCollection<ItemDto> TodoItems { get; } = [];
        public ObservableCollection<ItemDto> DoneItems { get; } = [];

        public MainViewModel(ItemService itemService)
        {
            Title = "Todo list";
            _itemService = itemService;
            TodoItems = new(Items.Where(x => !x.IsChecked));
            DoneItems = new(Items.Where(x => x.IsChecked));
        }

        [RelayCommand]
        public async Task GetItems()
        {
            var items = await _itemService.GetAllItemsAsync();
             foreach(var item in items)
            {
                Attach(item);
                Items.Add(item);
                if (item.IsChecked)
                {
                    DoneItems.Add(item);
                }
                else
                {
                    TodoItems.Add(item);
                }
            }
        }

        [RelayCommand]
        public async Task OpenPopup()
        {
            ItemCreateViewModel itemCreateViewModel = new ItemCreateViewModel() { IsEdit = false };
            var result = (ItemEditResult?)await MauiApp.Current.MainPage.ShowPopupAsync(new ItemCreatePage(itemCreateViewModel));
            if(result?.Action == ItemEditAction.Save && !string.IsNullOrWhiteSpace(result.Text))
            {
                var newItem = new ItemDto { Text = result.Text };
                await AddItem(newItem);
                await _itemService.AddItemAsync(newItem);
            }

        }

        [RelayCommand]
        public async Task EditItem(ItemDto item)
        {
            // pass existing text into popup VM
            ItemCreateViewModel itemCreateViewModel = new ItemCreateViewModel() { IsEdit = true, Text = item.Text };
            var popup = new ItemCreatePage(itemCreateViewModel);

            var result = (ItemEditResult?)await MauiApp.Current.MainPage.ShowPopupAsync(new ItemCreatePage(itemCreateViewModel));
            if (result?.Action == ItemEditAction.Save && !string.IsNullOrWhiteSpace(result.Text))
            {
                item.Text = result.Text;
                await _itemService.UpdateItemAsync(item);
            }
            else if (result.Action.Equals(ItemEditAction.Delete))
            {
                await RemoveItem(item);
                await _itemService.DeleteItemAsync(item.Id);
            }
        }

        public async Task AddItem(ItemDto item)
        {
            Attach(item);
            Items.Add(item);
            TodoItems.Add(item);
        }

        public async Task RemoveItem(ItemDto item)
        {
            Detach(item);
            Items.Remove(item);
            TodoItems.Remove(item);
            DoneItems.Remove(item);
        }

        public async Task ChackedChanged(ItemDto item)
        {
            if (item.IsChecked)
            {
                await Checked(item);
            }

            if (!item.IsChecked)
            {
                await UnChecked(item);
            }
        }

        private async Task Checked(ItemDto item)
        {
            item.DateDone = DateTime.UtcNow;
            TodoItems.Remove(item);
            DoneItems.Add(item);
            await _itemService.UpdateItemAsync(item);
        }

        private async Task UnChecked(ItemDto item)
        {
            TodoItems.Add(item);
            DoneItems.Remove(item);
            await _itemService.UpdateItemAsync(item);
        }



        private void Attach(ItemDto item) => item.PropertyChanged += OnItemPropertyChanged;
        private void Detach(ItemDto item) => item.PropertyChanged -= OnItemPropertyChanged;

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not ItemDto item) return;
            if (e.PropertyName == nameof(ItemDto.IsChecked))
            {
                // osiguraj se da si na UI threadu
                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(async () =>
                   await ChackedChanged(item)
                );
            }
        }

    }
}
