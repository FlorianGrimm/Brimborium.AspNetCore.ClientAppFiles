using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

//using AngleSharp.Html.Dom;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Options;

using WebApp;
//using RazorPagesProject.Tests.Helpers;

using Xunit;
namespace Brimborium.AspNetCore.ClientAppFiles.Test;

public class UnitTest1 :
    IClassFixture<CustomWebApplicationFactory<WebApp.Program>> {
    private readonly CustomWebApplicationFactory<Program>
        _factory;

    public UnitTest1(
        CustomWebApplicationFactory<Program> factory) {
        _factory = factory;
    }

    [Fact]
    public async Task TestenUSIndexHtml() {
        void ConfigureTestServices(IServiceCollection services) {
            var authenticationBuilder = services.AddAuthentication(defaultScheme: "TestScheme");
            authenticationBuilder.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
        }

        var client = _factory
               .WithWebHostBuilder(builder =>
                   builder.ConfigureTestServices(ConfigureTestServices))
               .CreateClient(new WebApplicationFactoryClientOptions {
                   AllowAutoRedirect = false,
               });

        using var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/en-US/index.html"));
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("en-US", content);
    }
}
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions> {
    public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder) {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
        var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}