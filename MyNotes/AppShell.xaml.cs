namespace MyNotes
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            FlyoutBehavior = (DeviceInfo.Idiom == DeviceIdiom.Desktop || DeviceInfo.Idiom == DeviceIdiom.Tablet)
                        ? FlyoutBehavior.Locked
                        : FlyoutBehavior.Flyout;
        }
    }
}
