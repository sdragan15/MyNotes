using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Domain.Entities
{
    public class Notes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDone { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public Notes()
        {
            DateCreated = DateTime.UtcNow;
            DateDone = DateTime.UtcNow;
            LastUpdateTime = DateTime.UtcNow;
        }
    }
}
