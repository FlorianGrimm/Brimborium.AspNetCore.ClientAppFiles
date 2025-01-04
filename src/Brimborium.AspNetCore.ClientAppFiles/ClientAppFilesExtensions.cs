using Microsoft.Extensions.Configuration;

namespace Brimborium.AspNetCore.ClientAppFiles;

/// <summary>
/// Extension methods for the ClientAppFilesOptions
/// </summary>
public static class ClientAppFilesExtensions {

    /// <summary>
    /// Binds the configuration to the options
    /// </summary>
    /// <param name="options">the target</param>
    /// <param name="configuration">the source</param>
    public static void Bind(this ClientAppFilesOptions options, IConfiguration configuration) {
        if (configuration.GetSection(nameof(options.ListRequestPath)).GetChildren() is { } listChild
            && listChild.Any()) {
            var listRequestPath = new List<PathDocument>();
            foreach (var child in listChild) {
                if (child.Key is { } path) {
                    if (!(child.Value is { Length: > 0 } document)) {
                        document = path + "/index.html";
                    }
                    listRequestPath.Add(new PathDocument(path, document));
                }
            }
            options.ListRequestPath = listRequestPath.ToArray();
        }
        if (configuration.GetSection(nameof(options.DefaultFile)).Value is { } valueFile) {
            options.DefaultFile = valueFile;
        }
        if (configuration.GetSection(nameof(options.Policy)).Value is { } valuePolicy) {
            options.Policy = valuePolicy;
        }
    }
}
