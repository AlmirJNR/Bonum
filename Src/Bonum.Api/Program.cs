using Bonum.Api.Clients;
using Bonum.Api.Services;
using Bonum.Contracts.Interfaces;
using Bonum.Contracts.Messages;
using Bonum.Shared.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Bonum.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddMassTransit(x =>
        {
            x.AddRequestClient<OcrMessage>(RequestTimeout.After(s:10));
            x.UsingBonumRabbitMq(builder.Configuration);
        });
        builder.Services.AddTransient<IAmqpClient<OcrMessage, OcrMessageResult>, OcrClient>();
        builder.Services.AddTransient<IOcrService, OcrService>();
        builder.Services.AddControllers(options =>
        {
            options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
        });

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
    }
}