using Brimborium.AspNetCore.ClientAppFiles;

using Microsoft.AspNetCore.Authentication.Negotiate;

namespace WebApp;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddLogging(loggingBuilder => {
            loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
            loggingBuilder.AddConsole(options => { });
        });
        builder.Services.AddOpenApi();
        if (builder.Environment.EnvironmentName == "Test") {
            builder.Services.AddAuthentication();
        } else { 
            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();
        }

        builder.Services.AddAuthorization(options => {
            /* 3. Set the authorization Policy - if you need*/
            // options.FallbackPolicy = options.DefaultPolicy;
            // - or -
            options.AddPolicy("RequireAuthenticatedUser", policy => policy.RequireAuthenticatedUser());
        });

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
                options.Policy = "RequireAuthenticatedUser";
                options.UseLocalizeDefaultFile = true;
            }
            );
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
        app.UseRequestLocalization();
    
        /* 2. MapClientAppFiles to map the client app files to the request path */
        app.MapClientAppFiles();

        /* 3. The js/css - assets are delivered by the StaticAssets middleware  */
        app.MapStaticAssets();

        app.Run();
    }
}
