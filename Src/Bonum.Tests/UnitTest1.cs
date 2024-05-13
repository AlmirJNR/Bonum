using System.Net.Http.Json;
using Bonum.Api;
using Bonum.Contracts.Messages;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using Bonum.Tests.Fixtures;

namespace Bonum.Tests;

public class UnitTest1 : IClassFixture<RabbitMqFixture>, IAsyncLifetime
{
    private readonly IFutureDockerImage _ocrImage;
    private readonly IContainer _ocrContainer;

    public UnitTest1(RabbitMqFixture rabbitMqFixture)
    {
        _ocrImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile("Bonum.Ocr.Dockerfile")
            .Build();

        _ocrContainer = new ContainerBuilder()
            .WithImage(_ocrImage)
            .WithEnvironment("DOTNET_ENVIRONMENT", "Container")
            .WithNetwork(rabbitMqFixture.Network)
            .Build();
    }

    [Theory]
    [InlineData("image.png", "Almir Júnior")]
    [InlineData("image2.png", "Almir A. F. Junior")]
    public async Task Test1(string fileName, string expectedValue)
    {
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
        });
        using var client = factory.CreateClient();

        var workingDirectory = Directory.GetCurrentDirectory();
        var binariesDirectory = Directory.GetParent(workingDirectory)!.Parent!.FullName;
        var projectDirectory = Directory.GetParent(binariesDirectory)!.FullName;

        var filePath = Path.Combine(projectDirectory, "Assets", fileName);
        var file = new FileInfo(filePath);

        using var memoryStream = new MemoryStream();
        await file.OpenRead().CopyToAsync(memoryStream);

        var fileByteContent = new ByteArrayContent(memoryStream.ToArray());
        fileByteContent.Headers.ContentType = MediaTypeHeaderValue.Parse(
            $"image/{file.Extension.Replace(".", string.Empty)}"
        );

        var formData = new MultipartFormDataContent();
        formData.Add(fileByteContent, "Value", file.Name);
        formData.Add(new StringContent("por"), "languages");

        using var response = await client.PostAsync("/api/v1/Ocr", formData);
        var result = await response.Content.ReadFromJsonAsync<OcrMessageResult>();

        Assert.Contains(expectedValue, result!.Text);
    }

    public async Task InitializeAsync()
    {
        await _ocrImage.CreateAsync();
        await _ocrContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _ocrImage.DisposeAsync();
        await _ocrContainer.StopAsync();
    }
}