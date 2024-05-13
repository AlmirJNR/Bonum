using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

namespace Bonum.Tests.Fixtures;

public class RabbitMqFixture : IAsyncLifetime
{
    public INetwork Network { get; }
    private IContainer RabbitMqContainer { get; }

    public RabbitMqFixture()
    {
        Network = new NetworkBuilder()
            .WithName(Guid.NewGuid().ToString())
            .Build();

        RabbitMqContainer = new ContainerBuilder()
            .WithImage("rabbitmq:3-alpine")
            .WithName("rabbitmq")
            .WithVolumeMount("rabbitmq.conf", "/etc/rabbitmq/rabbitmq.conf", AccessMode.ReadOnly)
            .WithNetwork(Network)
            .WithPortBinding(5672, 5672)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5672))
            .Build();
    }

    public async Task InitializeAsync()
    {
        await Network.CreateAsync();
        await RabbitMqContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Network.DisposeAsync();
        await RabbitMqContainer.StopAsync();
    }
}