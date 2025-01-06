# Brimborium.AspNetCore.ClientAppFiles

for AspNetCore deliver ClientApp files for a SinglePageApplication.

The supported workflow is the programmer start the npm/ng tool to generate the files, so the error messages are available in the editor.
The dotnet webapp is running and deliver the (new) files.

Their is no npm server running, no CORS, no proxy forward thing.

Windows authentication works since you are not using a proxy.

So you can restart the npm watch or the debugger running your webapp independently.

## Usage if you are using Angular with i18n

1. ClientAppFiles

    ```csharp
            builder.Services.AddClientAppFiles(
                /* configuration: builder.Configuration.GetSection("ClientAppFiles"),*/
                configureOptions: (options) => {
                    options.DefaultFile = new PathString("/en-US/index.html");
                    options.ListRequestPath = [
                        new PathDocument(new PathString("/de-DE"), new PathString("/de-DE/index.html")),
                        new PathDocument(new PathString("/en-US"), new PathString("/en-US/index.html")),
                        new PathDocument(new PathString("/fr-FR"), new PathString("/fr-FR/index.html")),
                        ];
                }
                );

            var app = builder.Build();

            // ...

            app.MapClientAppFiles();

            app.MapStaticAssets();
    ```

2. Authorization - Policy

    ```csharp
        builder.Services.AddAuthorization(options => {
            // options.FallbackPolicy = options.DefaultPolicy;
            // - or -
            options.AddPolicy("ClientAppFiles", policy => policy.RequireAuthenticatedUser());
        });

        builder.Services.AddClientAppFiles(
            configureOptions: (options) => {
                // ...
                /* 2. Use the Policy */
                options.Policy = "ClientAppFiles";
            }
            );
    ```

3. Localization
    ```csharp
        builder.Services.AddClientAppFiles(
            configureOptions: (options) => {
                // ...
                /* 3. Localize*/
                options.UseLocalizeOnRootPath = true;
            }
            );
        // ...
        app.UseRequestLocalization();
        app.MapClientAppFiles();
    ```

4. Angular angular.json
  
 
   projects/ClientApp/i18n : Added while in the i18n configuration.
  
   projects/ClientApp/architect/build: must be modified by you.
   "builder": "@angular-devkit/build-angular:browser" because this don't use Server-Side Rendering.
   options/outputPath: is set to the wwwroot folder of the dotnet webapp.

    ```json
    {
        "$schema": "./node_modules/@angular/cli/lib/config/schema.json",
        "version": 1,
        "newProjectRoot": "projects",
        "projects": {
          "ClientApp": {
            "projectType": "application",
            "schematics": {
              "@schematics/angular:component": {
                "style": "scss"
              }
            },
            "root": "",
            "sourceRoot": "src",
            "prefix": "app",
            "i18n": {
              "sourceLocale":"en-US",
              "locales": {
                "de-DE": "src/locale/messages.de-DE.xlf",
                "fr-FR": "src/locale/messages.fr-FR.xlf"
              }
            },
            "architect": {
              "build": {
                "builder": "@angular-devkit/build-angular:browser",
                "options": {
                  "localize": ["en-US"],
                  "baseHref": "/",
                  "outputPath": "../WebApp/wwwroot/",
                  "deleteOutputPath": false,
                  "index": "src/index.html",
                  "main": "src/main.ts",
                  "extractLicenses": false,
        ...
    }
    ```

Complete Example:

```csharp
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddOpenApi();

    var authenticationBuilder = builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme);
    authenticationBuilder.AddNegotiate();

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
```

Happy Coding