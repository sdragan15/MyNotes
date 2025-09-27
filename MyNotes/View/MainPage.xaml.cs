using MyNotes.ViewModel;

namespace MyNotes.View
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
            mainViewModel.GetItemsCommand.Execute(null);
        }
    }

}
