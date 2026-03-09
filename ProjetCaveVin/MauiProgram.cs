using EvoTrackBack.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ProjetCaveVin
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

            // Charger appsettings.json
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("ProjetCaveVin.appsettings.json");
            if (stream != null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
                builder.Configuration.AddConfiguration(config);
            }

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
            builder.Services.AddScoped<RARepository>();
            builder.Services.AddScoped<UtilisateurRepository>();
            builder.Services.AddScoped<RoleRepository>();
            builder.Services.AddScoped<HDRepository>();
            builder.Services.AddScoped<ZoneRepository>();
            builder.Services.AddScoped<BouteilleRepository>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}