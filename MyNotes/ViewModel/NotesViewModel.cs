using MyNotes.Application.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
