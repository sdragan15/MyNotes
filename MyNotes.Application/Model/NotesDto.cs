using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Application.Model
{
    public partial class NotesDto : ObservableObject
    {
        public Guid Id { get; set; }
        [ObservableProperty]
        string? content;
        [ObservableProperty]
        string? header;
        [ObservableProperty]
        public DateTime dateCreated;
        [ObservableProperty]
        public bool isSelected;
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public NotesDto()
        {
            Id = Guid.NewGuid();
            content = "New content";
            header = "New note";
            dateCreated = DateTime.UtcNow;
            isSelected = false;
        }
        
    }
}
