using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Model;
using MyNotes.Services;
using MyNotes.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyNotes.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        ItemService itemService;
        public ObservableCollection<Item> Items { get; } = [];
        public ObservableCollection<Item> TodoItems { get; } = [];
        public ObservableCollection<Item> DoneItems { get; } = [];

        public MainViewModel(ItemService itemService)
        {
            Title = "Todo list";
            this.itemService = itemService;
            TodoItems = new(Items.Where(x => !x.IsChecked));
            DoneItems = new(Items.Where(x => x.IsChecked));
        }

        [RelayCommand]
        public void GetItems()
        {
            TodoItems.Add(new Item()
            {
                Text = "This is new"
            });
        }

        [RelayCommand]
        public async void OpenPopup()
        {
            ItemCreateViewModel itemCreateViewModel = new ItemCreateViewModel() { IsEdit = false };
            var result = (ItemEditResult?)await Application.Current.MainPage.ShowPopupAsync(new ItemCreatePage(itemCreateViewModel));
            if(result?.Action == ItemEditAction.Save && !string.IsNullOrWhiteSpace(result.Text))
            {
                var newItem = new Item { Text = result.Text };
                AddItem(newItem);
            }

        }

        [RelayCommand]
        public async Task EditItem(Item item)
        {
            // pass existing text into popup VM
            ItemCreateViewModel itemCreateViewModel = new ItemCreateViewModel() { IsEdit = true, Text = item.Text };
            var popup = new ItemCreatePage(itemCreateViewModel);

            var result = (ItemEditResult?)await Application.Current.MainPage.ShowPopupAsync(new ItemCreatePage(itemCreateViewModel));
            if (result?.Action == ItemEditAction.Save && !string.IsNullOrWhiteSpace(result.Text))
            {
                item.Text = result.Text;
            }
            else if (result.Action.Equals(ItemEditAction.Delete))
            {
                RemoveItem(item);
            }
        }

        public void AddItem(Item item)
        {
            Attach(item);
            Items.Add(item);
            UnChecked(item);
        }

        public void RemoveItem(Item item)
        {
            Detach(item);
            Items.Remove(item);
            Checked(item);
        }

        public void ChackedChanged(Item item)
        {
            if (item.IsChecked)
            {
                Checked(item);
            }

            if (!item.IsChecked)
            {
                UnChecked(item);
            }
        }

        private void Checked(Item item)
        {
            TodoItems.Remove(item);
            DoneItems.Add(item);
        }

        private void UnChecked(Item item)
        {
            TodoItems.Add(item);
            DoneItems.Remove(item);
        }



        private void Attach(Item item) => item.PropertyChanged += OnItemPropertyChanged;
        private void Detach(Item item) => item.PropertyChanged -= OnItemPropertyChanged;

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not Item item) return;
            if (e.PropertyName == nameof(Item.IsChecked))
            {
                // osiguraj se da si na UI threadu
                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
                   ChackedChanged(item)
                );
            }
        }

    }
}
