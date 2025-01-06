using Brimborium.AspNetCore.ClientAppFiles;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Maps the client app files to the request pipeline.
/// </summary>
public static class ClientAppFilesBuilderExtensions {

#if NET9_0_OR_GREATER

    private static readonly string[] _supportedHttpMethods = new[] { HttpMethods.Get, HttpMethods.Head };

    /// <summary>
    /// Maps the client app files to the request pipeline.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> (the app-variable in Program.cs) </param>
    /// <param name="staticFileOptions">(Normally) not needed</param>
    /// <returns>fluent this.</returns>
    public static IEndpointRouteBuilder MapClientAppFiles(
        this WebApplication app,
        StaticFileOptions? staticFileOptions = null) {
        var clientAppFilesOptions = app.Services.GetRequiredService<IOptions<ClientAppFilesOptions>>().Value;
        if (clientAppFilesOptions is null) {
            throw new InvalidOperationException("Missing ClientAppFilesOptions.");
        }
        var order = clientAppFilesOptions.Order;

        foreach (var pathDocument in clientAppFilesOptions.ListRequestPath) {
            {
                var endpointConventionBuilder = app
                    .Map(pathDocument.Path, CreateRequestDelegate(app, pathDocument.Document, staticFileOptions))
                    .WithMetadata(new HttpMethodMetadata(_supportedHttpMethods))
                    .WithOrder(order);
                if (clientAppFilesOptions.Policy is { Length: > 0 } policy) {
                    endpointConventionBuilder.RequireAuthorization(policy);
                }
            }
            {
                var endpointConventionBuilder = app
                    .Map((pathDocument.Path.Value + "/{*path:nonfile}"), CreateRequestDelegate(app, pathDocument.Document, staticFileOptions))
                    .WithMetadata(new HttpMethodMetadata(_supportedHttpMethods))
                    .WithOrder(order);
                if (clientAppFilesOptions.Policy is { Length: > 0 } policy) {
                    endpointConventionBuilder.RequireAuthorization(policy);
                }
            }
        }

        if (clientAppFilesOptions.UseLocalizeDefaultFile) {
            var endpointConventionBuilder = app
                .Map("/", CreateRequestDelegateLocalize(app, clientAppFilesOptions.DefaultFile, clientAppFilesOptions.ListRequestPath, staticFileOptions))
                .WithMetadata(new HttpMethodMetadata(_supportedHttpMethods))
                .WithOrder(order);
            if (clientAppFilesOptions.Policy is { Length: > 0 } policy) {
                endpointConventionBuilder.RequireAuthorization(policy);
            }
        } else {
            var endpointConventionBuilder = app
                .Map("/", CreateRequestDelegate(app, clientAppFilesOptions.DefaultFile, staticFileOptions))
                .WithMetadata(new HttpMethodMetadata(_supportedHttpMethods))
                .WithOrder(order);
            if (clientAppFilesOptions.Policy is { Length: > 0 } policy) {
                endpointConventionBuilder.RequireAuthorization(policy);
            }
        }
        return app;
    }

    private static RequestDelegate CreateRequestDelegate(
        IEndpointRouteBuilder endpoints,
        string filePath,
        StaticFileOptions? options = null) {
        
        var app = endpoints.CreateApplicationBuilder();
        app.Use(next => context => {

            context.Request.Path = EnsureStartsWithSlash(filePath);

            // Set endpoint to null so the static files middleware will handle the request.
            context.SetEndpoint(null);

            return next(context);
        });

        if (options == null) {
            app.UseStaticFiles();
        } else {
            app.UseStaticFiles(options);
        }

        return app.Build();
    }

    private static RequestDelegate CreateRequestDelegateLocalize(
        IEndpointRouteBuilder endpoints,
        string defaultFilePath,
        PathDocument[] listRequestPath,
        StaticFileOptions? options = null) {        
        var app = endpoints.CreateApplicationBuilder();
        app.Use(next => context => {

            // Try to find the localized path (and file).
            bool found = false;
            var culture = context.Features.Get<IRequestCultureFeature>()?
                .RequestCulture?
                .UICulture.Name;
            if (culture is { Length: > 0 }) {
                var culturePathLength =/* Slash */1 + culture.Length;
                foreach (var pathDocument in listRequestPath) {
                    if ((pathDocument.Path.Value is { } pathCulture)
                        && pathCulture.Length == culturePathLength
                        && pathCulture.AsSpan(1).StartsWith(culture.AsSpan())) {
                        context.Request.Path = EnsureStartsWithSlash(pathDocument.Document);
                        found = true;
                        break;
                    }
                }
            }
            if (!found) {
                context.Request.Path = EnsureStartsWithSlash(defaultFilePath);
            }

            // Set endpoint to null so the static files middleware will handle the request.
            context.SetEndpoint(null);

            return next(context);
        });

        if (options == null) {
            app.UseStaticFiles();
        } else {
            app.UseStaticFiles(options);
        }

        return app.Build();
    }

    private static string EnsureStartsWithSlash(string filePath) {
        return filePath.StartsWith('/') ? (filePath) : ("/" + filePath);
    }


#else

// Dead weight?

    /// <summary>
    /// Uses a middleware to redirect the path to the client app files. You have to add the StaticFiles middleware to the pipeline (app.UseStaticFiles()).
    /// </summary>
    /// <param name="app">the app from program.cs</param>
    /// <returns>fluent this</returns>
    public static IApplicationBuilder UseClientAppFiles(
        this IApplicationBuilder app
        ) {
        var clientAppFilesOptions = app.ApplicationServices.GetRequiredService<IOptions<ClientAppFilesOptions>>().Value;
        app.UseMiddleware<ClientAppFilesMiddleware>(clientAppFilesOptions);
        return app;
    }

#endif

}
