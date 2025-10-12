using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Application.Model;
using MyNotes.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp = Microsoft.Maui.Controls.Application;

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

        public bool CanCreate => !string.IsNullOrWhiteSpace(Body) && !string.IsNullOrWhiteSpace(HeadTitle);

        public NotesCreateViewModel()
        {
            
        }

        [RelayCommand]
        private async Task CreateAsync()
        {
            var opId = Guid.NewGuid().ToString("N");
            await Shell.Current.GoToAsync("..", new Dictionary<string, object>
            {
                ["action"] = originalId == Guid.Empty ? "create" : "update",
                ["id"] = originalId.ToString(),
                ["opId"] = opId,
                ["createdText"] = Body?.Trim() ?? string.Empty,
                ["headTitle"] = HeadTitle?.Trim() ?? string.Empty
            });

            Reset();
        }

        [RelayCommand]
        private Task CancelAsync() => Shell.Current.GoToAsync("..");

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            NotesDto? originalItem;

            bool hasItem = query.TryGetValue("item", out var itemObj);
            
            if (!hasItem)
            {
                return;
            }

            originalItem = JsonSerializer.Deserialize<NotesDto>(itemObj?.ToString() ?? string.Empty);

            if (originalItem == null)
            {
                return;
            }

            PopulateFrom(originalItem);

        }

        public void PopulateFrom(NotesDto originalItem)
        {
            originalId = originalItem.Id;
            HeadTitle = originalItem.Header;
            Body = originalItem.Content;
        }

        private void Reset()
        {
            originalId = Guid.Empty;
            HeadTitle = "";
            Body = "";
        }
    }
}
