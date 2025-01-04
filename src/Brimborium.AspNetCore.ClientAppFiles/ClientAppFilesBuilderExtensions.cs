using Brimborium.AspNetCore.ClientAppFiles;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticAssets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder;

public static class ClientAppFilesBuilderExtensions {
    private static readonly string[] _supportedHttpMethods = new[] { HttpMethods.Get, HttpMethods.Head };

    public static IEndpointRouteBuilder MapClientAppFiles(
        this IEndpointRouteBuilder endpoints,
        ClientAppFilesOptions? clientAppFilesOptions = default,
        StaticFileOptions? staticFileOptions = null) {
        var app = endpoints as IApplicationBuilder;
        clientAppFilesOptions ??= app?.ApplicationServices.GetRequiredService<IOptions<ClientAppFilesOptions>>().Value;
        if (clientAppFilesOptions is null) { return endpoints; }

        var suffix = new PathString("/{*path:nonfile}");
        foreach (var pathDocument in clientAppFilesOptions.ListRequestPath) {
            endpoints
                .Map(pathDocument.Path, CreateRequestDelegate(endpoints, pathDocument.Document, staticFileOptions))
                .WithMetadata(new HttpMethodMetadata(_supportedHttpMethods));

            endpoints
                .Map(pathDocument.Path + suffix, CreateRequestDelegate(endpoints, pathDocument.Document, staticFileOptions))
                .WithMetadata(new HttpMethodMetadata(_supportedHttpMethods));
        }

        endpoints.Map("/", CreateRequestDelegate(endpoints, clientAppFilesOptions.DefaultFile, staticFileOptions))
            .WithMetadata(new HttpMethodMetadata(_supportedHttpMethods));

        app?.UseMiddleware<ClientAppFilesMiddleware>(clientAppFilesOptions);

        return endpoints;
    }

    private static RequestDelegate CreateRequestDelegate(
      IEndpointRouteBuilder endpoints,
      string filePath,
      StaticFileOptions? options = null) {
        var app = endpoints.CreateApplicationBuilder();
        app.Use(next => context => {

            context.Request.Path = filePath.StartsWith('/') ? (filePath) : ("/" + filePath);

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
}
