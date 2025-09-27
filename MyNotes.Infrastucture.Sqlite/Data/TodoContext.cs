using Microsoft.EntityFrameworkCore;
using MyNotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Infrastructure.Sqlite.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext()
        {
        }

        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "MyNotesDb.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
