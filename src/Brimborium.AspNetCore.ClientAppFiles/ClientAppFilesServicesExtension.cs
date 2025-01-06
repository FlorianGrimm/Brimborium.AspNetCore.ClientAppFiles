using Brimborium.AspNetCore.ClientAppFiles;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for the ClientAppFilesOptions
/// </summary>
public static class ClientAppFilesServicesExtension {

    /// <summary>
    /// Adds the client app files services to the service collection.
    /// </summary>
    /// <param name="services">this ServiceCollection</param>
    /// <param name="configuration">optional the configuration that should be copied into the options</param>
    /// <param name="configureOptions">optional the callback to configure the options</param>
    /// <returns>fluent this.</returns>
    public static IServiceCollection AddClientAppFiles(
        this IServiceCollection services,
        IConfiguration? configuration = default,
        Action<ClientAppFilesOptions>? configureOptions = default
    ) {
        if (configuration is { } || configureOptions is { }) {
            services.Configure<ClientAppFilesOptions>((options) => {
                if (configuration is { }) {
                    options.Bind(configuration);
                }
                if (configureOptions is { }) {
                    configureOptions.Invoke(options);
                }
            });
        }
        return services;
    }
}