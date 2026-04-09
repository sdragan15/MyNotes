using MyNotes.ViewModel;

namespace MyNotes
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            FlyoutBehavior = (DeviceInfo.Idiom == DeviceIdiom.Desktop || DeviceInfo.Idiom == DeviceIdiom.Tablet)
                        ? FlyoutBehavior.Locked
                        : FlyoutBehavior.Flyout;

            Routing.RegisterRoute(nameof(View.NotesCreatePage), typeof(View.NotesCreatePage));
        }
    }
}
