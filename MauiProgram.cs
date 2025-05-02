using Furia_FanHub.MVVM.Helpers;
using Furia_FanHub.MVVM.Models;
using Furia_FanHub.MVVM.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Furia_FanHub
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Configurar SQLite e injeção de dependência do FanRepository
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fans.db3");
            builder.Services.AddSingleton(new FanRepository(dbPath));

            return builder.Build();
        }
    }
}
