using CommunityToolkit.Maui.Views;
using MyNotes.ViewModel;

namespace MyNotes.View
{
    public partial class ItemCreatePage : Popup
    {
        public ItemCreatePage(ItemCreateViewModel itemCreateViewModel)
        {
            InitializeComponent();
            BindingContext = itemCreateViewModel;
            itemCreateViewModel.CloseRequested += (_, result) => Close(result);
            Opened += ItemCreatePage_Opened;
        }

        private void ItemCreatePage_Opened(object? sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(50);
                Input.Focus();
                var len = Input?.Text?.Length ?? 0;
                Input.CursorPosition = len;
            });
        }
    }
}

