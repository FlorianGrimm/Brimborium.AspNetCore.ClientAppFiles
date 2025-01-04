using Brimborium.AspNetCore.ClientAppFiles;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ClientAppFilesServicesExtension {
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