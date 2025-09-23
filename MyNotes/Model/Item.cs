using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Model
{
    public partial class Item : ObservableObject
    {
        public Guid Id { get; set; }
        [ObservableProperty]
        string? text;
        [ObservableProperty]
        public bool isChecked;

        public Item()
        {
            Id = Guid.NewGuid();
            text = string.Empty;
            isChecked = false;
        }
    }
}
