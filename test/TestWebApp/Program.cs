using System.Runtime.CompilerServices;

using Brimborium.AspNetCore.ClientAppFiles;

using Microsoft.AspNetCore.Authentication.Negotiate;

[assembly: InternalsVisibleTo("Brimborium.AspNetCore.ClientAppFiles.Test")]

namespace WebApp;

public class Program {
    internal static bool IsTest = false;

    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddLogging(loggingBuilder => {
            loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
            loggingBuilder.AddConsole(options => { });
        });
        builder.Services.AddOpenApi();

        if (IsTest == false) {
            var authenticationBuilder = builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme);
            authenticationBuilder.AddNegotiate();
        }

        /* 1. AddClientAppFiles to set the options via configuration or hard wired, as here. */
        builder.Services.AddClientAppFiles(
            /* configuration: builder.Configuration.GetSection("ClientAppFiles"),*/
            configureOptions: (options) => {
                options.DefaultFile = new PathString("/en-US/index.html");
                options.ListRequestPath = [
                    new PathDocument(new PathString("/de-DE"), new PathString("/de-DE/index.html")),
                    new PathDocument(new PathString("/en-US"), new PathString("/en-US/index.html")),
                    new PathDocument(new PathString("/fr-FR"), new PathString("/fr-FR/index.html")),
                    new PathDocument(new PathString("/de"), new PathString("/de-DE/index.html")),
                    new PathDocument(new PathString("/en"), new PathString("/en-US/index.html")),
                    new PathDocument(new PathString("/fr"), new PathString("/fr-FR/index.html")),
                    ];
                /* 2. Use the Policy */
                options.Policy = "ClientAppFiles";
                /* 3. Localize*/
                options.UseLocalizeOnRootPath = true;
            }
            );

        /* 2. Set the authorization Policy - if you need*/
        builder.Services.AddAuthorization(options => {
            // options.FallbackPolicy = options.DefaultPolicy;
            // - or -
            options.AddPolicy("ClientAppFiles", policy => policy.RequireAuthenticatedUser());
        });

        /* 3. Localize */
        builder.Services.Configure<RequestLocalizationOptions>(options => {
            var requestCultureProviders = options.RequestCultureProviders;
            var supportedCultures = new[] { "en-US", "fr-FR", "de-DE", "en", "fr", "de" };
            options.SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);
        });
        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        /* 3. Localize*/
        app.UseRequestLocalization();

        /* 1. MapClientAppFiles to map the client app files to the request path */
        app.MapClientAppFiles();

        /* 1. The js/css - assets are delivered by the StaticAssets middleware  */
        app.MapStaticAssets();

        app.Run();
    }
}
