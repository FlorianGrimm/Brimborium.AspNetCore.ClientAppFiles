using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

using System.Net;
using System.Net.Http.Headers;

using WebApp;
namespace Brimborium.AspNetCore.ClientAppFiles.Test;

public class GetIndexWithLocalsTest :
    IClassFixture<CustomWebApplicationFactory<WebApp.Program>> {
    private readonly CustomWebApplicationFactory<Program> _factory;

    public GetIndexWithLocalsTest(
        CustomWebApplicationFactory<Program> factory
        ) {
        _factory = factory;
    }

    [Fact]
    public async Task GetRootWithNoLocals() {
        void ConfigureTestServices(IServiceCollection services) {
        }

        var client = _factory
               .WithWebHostBuilder(builder =>
                   builder.ConfigureTestServices(ConfigureTestServices))
               .CreateClient(new WebApplicationFactoryClientOptions {
                   AllowAutoRedirect = false,
               });

        var request = new HttpRequestMessage(HttpMethod.Get, "");
        using var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("en-US", content);
    }

    [Fact]
    public async Task GetRootWithLocalsdeDE() {
        void ConfigureTestServices(IServiceCollection services) {
        }

        var client = _factory
               .WithWebHostBuilder(builder =>
                   builder.ConfigureTestServices(ConfigureTestServices))
               .CreateClient(new WebApplicationFactoryClientOptions {
                   AllowAutoRedirect = false,
               });
        var request = new HttpRequestMessage(HttpMethod.Get, "");
        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("de-DE", 1));
        request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US", 0.7));
        using var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("de-DE", content);
    }

    [Fact]
    public async Task GetenUS() {
        void ConfigureTestServices(IServiceCollection services) {
        }

        var client = _factory
               .WithWebHostBuilder(builder =>
                   builder.ConfigureTestServices(ConfigureTestServices))
               .CreateClient(new WebApplicationFactoryClientOptions {
                   AllowAutoRedirect = false,
               });

        var request = new HttpRequestMessage(HttpMethod.Get, "/en-US");
        using var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("en-US", content);
    }


    [Fact]
    public async Task GetenUSIndexHtml() {
        void ConfigureTestServices(IServiceCollection services) {
        }

        var client = _factory
               .WithWebHostBuilder(builder =>
                   builder.ConfigureTestServices(ConfigureTestServices))
               .CreateClient(new WebApplicationFactoryClientOptions {
                   AllowAutoRedirect = false,
               });

        var request = new HttpRequestMessage(HttpMethod.Get, "/en-US/index.html");
        using var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("en-US", content);
    }


    [Fact]
    public async Task GetIndexHtmlAs404() {
        void ConfigureTestServices(IServiceCollection services) {
        }

        var client = _factory
               .WithWebHostBuilder(builder =>
                   builder.ConfigureTestServices(ConfigureTestServices))
               .CreateClient(new WebApplicationFactoryClientOptions {
                   AllowAutoRedirect = false,
               });

        var request = new HttpRequestMessage(HttpMethod.Get, "/index.html");
        using var response = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
