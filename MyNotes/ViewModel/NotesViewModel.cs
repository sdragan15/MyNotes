using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using MyNotes.Application.Model;
using MyNotes.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace MyNotes.ViewModel
{
    public partial class NotesViewModel : BaseViewModel
    {
        public ObservableCollection<NotesDto> NoteItems { get; } = [];

        public NotesViewModel()
        {
            NoteItems.Add(new NotesDto { Header = "First Note", Content = "This is the content of the first note.", DateCreated = DateTime.Now });
            NoteItems.Add(new NotesDto { Header = "Second Note", Content = "This is the content of the second note.", DateCreated = DateTime.Now });
            NoteItems.Add(new NotesDto { Header = "Third Note", Content = "This is the content of the third note.", DateCreated = DateTime.Now });
        }

        [RelayCommand]
        public async Task EditItem(NotesDto item)
        {
            NotesCreateViewModel notesCreateViewModel = new NotesCreateViewModel();
            var popup = new NotesCreatePage(notesCreateViewModel);

            var result = await MauiApp.Current.MainPage.ShowPopupAsync(popup);
            if (result == null) return;
        }
    }
}
