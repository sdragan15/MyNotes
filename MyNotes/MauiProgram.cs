using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyNotes.Application.Services;
using MyNotes.Domain.Interfaces;
using MyNotes.Infrastructure.Sqlite.Data;
using MyNotes.Infrastucture.Sqlite.Repositories;
using MyNotes.View;
using MyNotes.ViewModel;

namespace MyNotes
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<ItemCreateViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ItemCreatePage>();
            builder.Services.AddScoped<IItemRepository, ItemRepository>();
            builder.Services.AddScoped<ItemService>();

            #region Database
            
            var dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "MyNotesDb.db");
            builder.Services.AddDbContext<TodoContext>();

            #endregion
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
