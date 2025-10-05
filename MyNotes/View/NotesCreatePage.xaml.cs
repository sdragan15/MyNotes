using CommunityToolkit.Maui.Views;
using MyNotes.ViewModel;

namespace MyNotes.View
{
    public partial class NotesCreatePage : Popup
    {
        public NotesCreatePage(NotesCreateViewModel notesCreateViewModel)
        {
            InitializeComponent();
            BindingContext = notesCreateViewModel;
        }
    }
}

