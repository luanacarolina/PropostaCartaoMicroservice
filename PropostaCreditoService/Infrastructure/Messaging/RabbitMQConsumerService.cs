using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropostaCreditoService.Infrastructure.Messaging
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly PropostaDeCreditoService _propostaService;

        public RabbitMQConsumerService(PropostaDeCreditoService propostaService)
        {
            _propostaService = propostaService;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "clienteExchange", type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: "clienteQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: "clienteQueue", exchange: "clienteExchange", routingKey: "clienteRoutingKey");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var cliente = JsonSerializer.Deserialize<Cliente>(message);

                await ProcessarProposta(cliente);
            };

            _channel.BasicConsume(queue: "clienteQueue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private Task ProcessarProposta(Cliente cliente)
        {
            var proposta = new PropostaDeCredito
            {
                ClienteId = cliente.Id,
                Limite = 1000.00m // Exemplo de lógica de negócio
            };

            _propostaService.AddProposta(proposta);

            Console.WriteLine($"Proposta de crédito gerada para o cliente {cliente.Nome} com limite {proposta.Limite}");

            // Envia a proposta para o próximo serviço
            EnviarMensagemProposta(proposta);

            return Task.CompletedTask;
        }

        private void EnviarMensagemProposta(PropostaDeCredito proposta)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "propostaExchange", type: ExchangeType.Direct);
            channel.QueueDeclare(queue: "propostaQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "propostaQueue", exchange: "propostaExchange", routingKey: "propostaRoutingKey");

            var propostaJson = JsonSerializer.Serialize(proposta);
            var body = Encoding.UTF8.GetBytes(propostaJson);
            channel.BasicPublish(exchange: "propostaExchange", routingKey: "propostaRoutingKey", basicProperties: null, body: body);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}