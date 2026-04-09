using Microsoft.EntityFrameworkCore;
using MyNotes.Infrastructure.Sqlite.Data;
using MauiApp = Microsoft.Maui.Controls.Application;

namespace MyNotes
{
    public partial class App : MauiApp
    {
        private readonly TodoContext _todoContext;

        public App(TodoContext todoContext, AppShell appShell)
        {
            InitializeComponent();

            _todoContext = todoContext;
            _todoContext.Database.Migrate();

            MainPage = appShell;
        }
    }
}
