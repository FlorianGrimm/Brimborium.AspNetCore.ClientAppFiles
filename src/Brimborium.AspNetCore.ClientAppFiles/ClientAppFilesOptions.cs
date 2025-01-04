using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Brimborium.AspNetCore.ClientAppFiles;

/// <summary>
/// Options for the client app files middleware
/// </summary>
public class ClientAppFilesOptions {
    
    /// <summary>
    /// The request path that maps to static resources
    /// </summary>
    public PathDocument[] ListRequestPath { get; set; } = [];

    /// <summary>
    /// The default file that maps to the root request path and any other request path that does not map to a static resource.
    /// </summary>
    public PathString DefaultFile { get; set; } = new PathString("/index.html");

    /// <summary>
    /// The policy name - if set, the policy is required for the request paths
    /// </summary>
    public string? Policy { get; set; }

    /// <summary>
    /// The order of the added endpoints.
    /// </summary>
    public int Order { get; set; } = 100;

    /// <summary>
    /// The file system used to locate resources
    /// </summary>
    public IFileProvider? FileProvider { get; set; }

}
