using Microsoft.EntityFrameworkCore;
using MyNotes.Domain.Entities;

namespace MyNotes.Infrastructure.Sqlite.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext()
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<NoteCategory> NoteCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MyNotesDb.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notes>()
                .HasOne(n => n.Category)
                .WithMany()
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<NoteCategory>().HasData(
                new NoteCategory { Id = 1, Name = "No category" },
                new NoteCategory { Id = 2, Name = "Work" },
                new NoteCategory { Id = 3, Name = "Home" },
                new NoteCategory { Id = 4, Name = "Grocery" }
            );
        }
    }
}
