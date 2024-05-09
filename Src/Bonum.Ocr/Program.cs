using System.Reflection;
using Bonum.Shared.Extensions;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(Assembly.GetExecutingAssembly());
    x.UsingBonumRabbitMq(builder.Configuration);
});

var host = builder.Build();
await host.RunAsync();