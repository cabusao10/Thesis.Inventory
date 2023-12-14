using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Thesis.Inventory.MobileApp.Configurations;
using Thesis.Inventory.MobileApp.Extensions;
using Thesis.Inventory.MobileApp.Popups;
using Microsoft.AspNetCore.SignalR.Client;

namespace Thesis.Inventory.MobileApp
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).UseMauiCommunityToolkit();

            var a = Assembly.GetExecutingAssembly();
            using (var stream = a.GetManifestResourceStream($"{Assembly.GetExecutingAssembly().GetName().Name}.appsettings.json"))
            {
                builder.Configuration.AddJsonStream(stream);
            }
#if ANDROID
Platforms.Android.DangerousAndroidMessageHandlerEmitter.Register();
Platforms.Android.DangerousTrustProvider.Register();
#endif
            var apiconfig = builder.Configuration.GetSection("ApiEndPoint");
            var apiconfiguration = new APIConfiguration();
            apiconfig.Bind(apiconfiguration);
            HttpClientHandler insecureHandler = GetInsecureHandler();
            builder.Services.AddViewModelLayer();
            builder.Services.AddScoped(s => apiconfiguration);
            
            builder.Services.AddScoped(sp => new HttpClient(insecureHandler) { BaseAddress = new Uri(apiconfig["BaseAddress"]) });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
        public static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
    }
}