using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Domain.Entities
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Text { get; set; }
        public bool IsChecked { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDone { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public Item()
        {
            Text = string.Empty;
            IsChecked = false;
            DateCreated = DateTime.UtcNow;
            DateDone = DateTime.UtcNow;
            LastUpdateTime = DateTime.UtcNow;
        }
    }
}
