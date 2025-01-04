using Brimborium.AspNetCore.ClientAppFiles;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Builder;

internal sealed class ClientAppFilesMiddleware {
    private readonly ClientAppFilesOptions _Options;
    private readonly RequestDelegate _Next;
    private readonly IWebHostEnvironment _HostingEnv;
    private readonly ILogger<ClientAppFilesMiddleware> _Logger;
    private readonly IFileProvider _FileProvider;

    public ClientAppFilesMiddleware(
        ClientAppFilesOptions options,
        RequestDelegate next,
        IWebHostEnvironment hostingEnv,
        ILogger<ClientAppFilesMiddleware> logger) {
        this._Options = options;
        this._Next = next;
        this._HostingEnv = hostingEnv;
        this._Logger = logger;
        this._FileProvider = options.FileProvider ?? hostingEnv.WebRootFileProvider ?? throw new InvalidOperationException("Missing FileProvider.");
    }

    public Task Invoke(HttpContext context) {
        var requestMethod = context.Request.Method;
        if (!IsGetOrHeadMethod(requestMethod)) {
            return this._Next(context);
        }
        if (context.WebSockets.IsWebSocketRequest) {
            return this._Next(context);
        }
        var requestPath = context.Request.Path;

        if (context.GetEndpoint() is { } endpoint) {
            _Logger.LogTrace("Redirect endpoint found: requestPath:{requestPath}; endpoint:{endpoint};", requestPath, endpoint.DisplayName);
            return this._Next(context);
        }
        if (!(requestPath.Value is { } requestPathValue) || string.Equals(requestPathValue, "/", StringComparison.Ordinal)) {
            context.Request.Path = this._Options.DefaultFile;
            _Logger.LogTrace("Redirect request: requestPath:{requestPath}; redirect:{redirect};", requestPath, context.Request.Path);
            return _Next(context);
        }
        var fileInfo = this._FileProvider.GetFileInfo(requestPathValue);
        if (fileInfo.Exists) {
            _Logger.LogTrace("File found: requestPath:{requestPath};", requestPath);
            return _Next(context);
        }

        foreach (var pathDocument in this._Options.ListRequestPath) {
            if (requestPath.StartsWithSegments(pathDocument.Path, StringComparison.Ordinal)) {
                var redirectPath = pathDocument.Document;
#if true

                context.Request.Path = redirectPath;
                _Logger.LogTrace("Redirect found: requestPath:{requestPath}; redirect:{redirect};", requestPath, redirectPath);
                return _Next(context);
#else
                context.Response.Redirect(redirectPath.Value!, true, true);
                _Logger.LogTrace("Redirect: requestPath:{requestPath}; redirect:{redirect} ", requestPath, redirectPath);
                return Task.CompletedTask;
#endif
            }
            if (requestPath.StartsWithSegments(pathDocument.Path, StringComparison.OrdinalIgnoreCase, out var remaining)) {
                var redirectPath = pathDocument.Path + remaining;
#if true
                context.Request.Path = redirectPath;
                _Logger.LogTrace("Redirect found: requestPath:{requestPath}; redirect:{redirect};", requestPath, redirectPath);
                return _Next(context);
#else
                context.Response.Redirect(redirectPath.Value!, true, true);
                _Logger.LogTrace("Redirect: requestPath:{requestPath}; redirect:{redirect} ", requestPath, redirectPath);
                return Task.CompletedTask;
#endif
            }
        }

        return _Next(context);
    }

    internal static bool IsGetOrHeadMethod(string method) {
        return HttpMethods.IsGet(method) || HttpMethods.IsHead(method);
    }

}