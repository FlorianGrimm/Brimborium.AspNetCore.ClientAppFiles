using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using System.Xml.Linq;

namespace Brimborium.AspNetCore.ClientAppFiles.Test;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class {
    public CustomWebApplicationFactory() {
        WebApp.Program.IsTest = true;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureServices((services) => {
            var authenticationBuilder = services.AddAuthentication(defaultScheme: "TestScheme");
            authenticationBuilder.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
        });

        builder.UseEnvironment("Test");
    }
}