IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Register services
        services.AddSingleton<PropostaDeCreditoService>();
        services.AddSingleton<IPropostaDeCreditoRepository, PropostaDeCreditoRepository>();
        services.AddHostedService<RabbitMQConsumerService>();
    })
    .Build();

await host.RunAsync();
