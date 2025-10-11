using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Application.Model;
using MyNotes.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace MyNotes.ViewModel
{
    public partial class NotesCreateViewModel : BaseViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCreate))]
        private string? body;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanCreate))]
        private string? headTitle;

        public bool CanCreate => !string.IsNullOrWhiteSpace(Body) && !string.IsNullOrWhiteSpace(HeadTitle);

        public NotesCreateViewModel()
        {

        }

        [RelayCommand]
        private async Task CreateAsync()
        {
            await Shell.Current.GoToAsync("..", new Dictionary<string, object>
            {
                ["createdText"] = Body?.Trim() ?? string.Empty,
                ["headTitle"] = HeadTitle?.Trim() ?? string.Empty
            });

            Reset();
        }

        [RelayCommand]
        private Task CancelAsync() => Shell.Current.GoToAsync("..");

        private void Reset()
        {
            HeadTitle = "";
            Body = "";
        }
    }
}
