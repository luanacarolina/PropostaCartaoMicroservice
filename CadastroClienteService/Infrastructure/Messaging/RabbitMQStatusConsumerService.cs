using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CadastroClienteService.Infrastructure.Messaging
{
    public class RabbitMQStatusConsumerService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQStatusConsumerService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "statusExchange", type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: "statusQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: "statusQueue", exchange: "statusExchange", routingKey: "statusRoutingKey");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var statusMessage = JsonSerializer.Deserialize<StatusMessage>(message);

                // Tratar mensagem de status
                Console.WriteLine($"Status recebido para o cliente {statusMessage.ClienteId}: {statusMessage.Status}");
                // Aqui você pode adicionar lógica para atualizar o status do cliente no banco de dados
            };

            _channel.BasicConsume(queue: "statusQueue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

    public class StatusMessage
    {
        public int ClienteId { get; set; }
        public string Status { get; set; }
    }
}
