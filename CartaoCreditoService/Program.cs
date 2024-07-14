var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Register services
        services.AddSingleton<CartaoDeCreditoService>();
        services.AddSingleton<ICartaoDeCreditoRepository, CartaoDeCreditoRepository>();
        services.AddHostedService<RabbitMQConsumerService>();
    })
    .Build();

await host.RunAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


