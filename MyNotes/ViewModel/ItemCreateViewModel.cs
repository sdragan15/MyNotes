using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.ViewModel
{
    public enum ItemEditAction { Save, Delete, Cancel }
    public sealed record ItemEditResult(ItemEditAction Action, string? Text);

    public partial class ItemCreateViewModel : BaseViewModel
    {
        public event EventHandler<ItemEditResult>? CloseRequested;

        [ObservableProperty]
        private string? text = String.Empty;
        [ObservableProperty]
        private bool isEdit;

        [RelayCommand]
        public void Done()
        {
            var value = Text?.Trim();
            CloseRequested?.Invoke(this, new(ItemEditAction.Save, value));
        }

        [RelayCommand]
        public void Delete()
        {
            CloseRequested?.Invoke(this, new(ItemEditAction.Delete, null));
        }
    }
}
