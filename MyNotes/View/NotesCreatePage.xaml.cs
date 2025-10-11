using CommunityToolkit.Maui.Views;
using MyNotes.ViewModel;

namespace MyNotes.View
{
    public partial class NotesCreatePage : ContentPage
    {
        public NotesCreatePage(NotesCreateViewModel notesCreateViewModel)
        {
            InitializeComponent();
            BindingContext = notesCreateViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(50);
                HeaderEntry.Focus();
                var len = HeaderEntry?.Text?.Length ?? 0;
                HeaderEntry.CursorPosition = len;
            });
        }
    }
}

