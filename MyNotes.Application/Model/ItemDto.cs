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
        public DateTime dateDone;

        public ItemDto()
        {
            Id = Guid.NewGuid();
            text = string.Empty;
            isChecked = false;
            dateCreated = DateTime.UtcNow;
        }
    }
}
