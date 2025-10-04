using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Application.Model
{
    public partial class ItemDto : ObservableObject
    {
        public Guid Id { get; set; }
        [ObservableProperty]
        string? text;
        [ObservableProperty]
        public bool isChecked;
        [ObservableProperty]
        public DateTime dateCreated;
        [ObservableProperty]
        public DateTime? dateDone;
        [ObservableProperty]
        public DateTime lastUpdateTime;

        public ItemDto()
        {
            Id = Guid.NewGuid();
            text = string.Empty;
            isChecked = false;
            dateCreated = DateTime.UtcNow;
            lastUpdateTime = DateTime.UtcNow;
        }

        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdateTime = DateTime.UtcNow;
        }

        public void Done()
        {
            IsChecked = true;
            DateDone = DateTime.UtcNow;
            LastUpdateTime = DateTime.UtcNow;
        }

        public void UnDone()
        {
            IsChecked = false;
            DateDone = null;
            LastUpdateTime = DateTime.UtcNow;
        }
    }
}
