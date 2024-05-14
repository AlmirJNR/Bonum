using System.Net.Http.Json;
using Bonum.Contracts.Messages;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Bonum.Tests.Fixtures;
using Bonum.Tests.Helpers;

namespace Bonum.Tests;

public class OcrApiTests : IClassFixture<CoreFixture>, IAsyncLifetime
{
    private readonly IFutureDockerImage _ocrImage;
    private readonly IContainer _ocrContainer;

    public OcrApiTests(CoreFixture coreFixture)
    {
        _ocrImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile("Bonum.Ocr.Dockerfile")
            .Build();

        _ocrContainer = new ContainerBuilder()
            .WithImage(_ocrImage)
            .WithEnvironment("DOTNET_ENVIRONMENT", "Container")
            .WithNetwork(coreFixture.Network)
            .Build();
    }

    [Theory]
    [InlineData("image.png", "Almir Júnior")]
    [InlineData("image2.png", "Almir A. F. Junior")]
    public async Task SendImageToApi_ShouldContainExpectedText(string fileName, string expectedText)
    {
        // Arrange
        var assetsDirectory = DirectoryHelper.GetAssetsDirectory();
        var filePath = Path.Combine(assetsDirectory, fileName);
        var file = FileHelper.GetFileInfo(filePath);
        using var memoryStream = await FileHelper.CopyFileToMemory(file);

        var formData = new MultipartFormDataContent();
        var byteArrayContent = MultipartFormDataHelper.CreateByteArrayContent(
            memoryStream,
            FileHelper.GetFileContentType(file)
        );
        formData.Add(byteArrayContent, "value", file.Name);
        formData.Add(new StringContent("por"), "languages");
        using var client = WebApplicationFactoryHelper.CreateDefaultClient();

        // Act
        using var response = await client.PostAsync("/api/v1/Ocr", formData);
        var result = await response.Content.ReadFromJsonAsync<OcrMessageResult>();

        // Assert
        Assert.Contains(expectedText, result!.Text);
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