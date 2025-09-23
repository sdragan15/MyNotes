using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MyNotes.Services;
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

            builder.Services.AddTransient<ItemService>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<ItemCreateViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ItemCreatePage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
