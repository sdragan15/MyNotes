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
using static Android.Preferences.PreferenceActivity;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace MyNotes.ViewModel
{
    public partial class NotesViewModel : BaseViewModel, IQueryAttributable
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
            await Shell.Current.GoToAsync(nameof(MyNotes.View.NotesCreatePage));
        }

        [RelayCommand]
        public async Task CreateItem()
        {
            await Shell.Current.GoToAsync(nameof(MyNotes.View.NotesCreatePage));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            string body = "";
            string title = "";

            if(query.TryGetValue("createdText", out var bodyObj))
            {
                body = bodyObj.ToString();
            }
            else
            {
                return;
            }

            if (query.TryGetValue("headTitle", out var titleObj))
            {
                title = titleObj.ToString();
            }
            else
            {
                return;
            }

            NoteItems.Insert(0, new NotesDto
            {
                Header = title,
                Content = body.Trim(),
                DateCreated = DateTime.Now
            });
        }
    }
}
