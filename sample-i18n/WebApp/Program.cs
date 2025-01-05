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
        builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();

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
                    ];
                options.Policy = "RequireAuthenticatedUser";
            }
            );

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        /* 2. MapClientAppFiles to map the client app files to the request path */
        app.MapClientAppFiles();

        /* 3. The js/css - assets are delivered by the StaticAssets middleware  */
        app.MapStaticAssets();

        {
            var groupAPI = app.MapGroup("/api");
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            groupAPI.MapGet("/weatherforecast", (HttpContext httpContext) => {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast(
                        Date: DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC: Random.Shared.Next(-20, 55),
                        Summary: summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .RequireAuthorization();

            groupAPI.MapGet("/username", (HttpContext httpContext) => {
                if (httpContext.User.Identity is { IsAuthenticated: true } identity) {
                    return identity.Name;
                } else {
                    return "Anonymous";
                }
            })
            .WithName("GetUserName")
            .RequireAuthorization();
        }

        app.Run();
    }
}
