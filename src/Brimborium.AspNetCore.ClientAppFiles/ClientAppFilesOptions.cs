using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Brimborium.AspNetCore.ClientAppFiles;

public class ClientAppFilesOptions {


    /// <summary>
    /// Defaults to all request paths.
    /// </summary>
    public ClientAppFilesOptions() {
    }

    /// <summary>
    /// The request path that maps to static resources
    /// </summary>
    public PathDocument[] ListRequestPath { get; set; } = [];

    public PathString DefaultFile { get; set; } = new PathString("/en-US/index.html");

    /// <summary>
    /// The file system used to locate resources
    /// </summary>
    public IFileProvider? FileProvider { get; set; }
}

public record PathDocument(PathString Path, PathString Document);