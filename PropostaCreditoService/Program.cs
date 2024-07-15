using PropostaCreditoService.Domain.Interfaces;
using PropostaCreditoService.Domain.Services;
using PropostaCreditoService.Infrastructure.Data;
using PropostaCreditoService.Infrastructure.Messaging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Register services
        services.AddSingleton<PropostaDeCreditoService>();
        services.AddSingleton<IPropostaDeCreditoRepository, PropostaDeCreditoRepository>();
        services.AddHostedService<RabbitMQConsumerService>();
        services.AddHostedService<RabbitMQStatusConsumerService>();
    })
    .Build();

await host.RunAsync();
