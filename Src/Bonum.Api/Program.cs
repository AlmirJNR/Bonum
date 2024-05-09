using Bonum.Api.Clients;
using Bonum.Api.Services;
using Bonum.Contracts.Interfaces;
using Bonum.Contracts.Messages;
using Bonum.Shared.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<OcrMessage>();
    x.UsingBonumRabbitMq(builder.Configuration);
});
builder.Services.AddTransient<IAmqpClient<OcrMessage, OcrMessageResult>, OcrClient>();
builder.Services.AddTransient<IOcrService, OcrService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Configuration.GetRequired<bool>("UseSwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();