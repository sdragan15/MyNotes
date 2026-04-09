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
            var vm = (NotesCreateViewModel)BindingContext;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(50);
                if (vm.IsEditMode)
                {
                    BodyEditor.Focus();
                    BodyEditor.CursorPosition = BodyEditor.Text?.Length ?? 0;
                }
                else
                {
                    HeaderEntry.Focus();
                    HeaderEntry.CursorPosition = HeaderEntry.Text?.Length ?? 0;
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ((NotesCreateViewModel)BindingContext).OnDisappearing();
        }
    }
}

