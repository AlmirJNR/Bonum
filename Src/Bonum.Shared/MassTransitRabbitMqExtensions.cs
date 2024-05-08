using Bonum.Shared.Configs;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Bonum.Shared;

public static class MassTransitRabbitMqExtensions
{
    public static void UsingBonumRabbitMq(this IBusRegistrationConfigurator configurator, IConfiguration configuration)
    {
        var mqConfig = new RabbitMqConfig(
            configuration.GetRequired<string>("RabbitMq:Host"),
            configuration.GetRequired<ushort>("RabbitMq:Port"),
            configuration.GetRequired<string>("RabbitMq:VirtualHost"),
            configuration.GetRequired<string>("RabbitMq:Username"),
            configuration.GetRequired<string>("RabbitMq:Password")
        );

        configurator.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(mqConfig.Host, mqConfig.Port, mqConfig.VirtualHost, h =>
            {
                h.Username(mqConfig.Username);
                h.Password(mqConfig.Password);
            });

            cfg.ConfigureEndpoints(ctx);
        });
    }
}