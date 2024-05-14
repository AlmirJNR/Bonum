using Bonum.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Bonum.Tests.Helpers;

public static class WebApplicationFactoryHelper
{
    public static HttpClient CreateDefaultClient()
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
        });
        var client = factory.CreateClient();
        return client;
    }
}