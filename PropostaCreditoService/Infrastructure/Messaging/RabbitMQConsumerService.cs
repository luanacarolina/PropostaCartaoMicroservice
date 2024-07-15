using PropostaCreditoService.Domain.Entities;
using PropostaCreditoService.Domain.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        private async Task ProcessarProposta(Cliente cliente)
        {
            var proposta = new PropostaDeCredito
            {
                ClienteId = cliente.Id,
                Limite = 1000.00m
            };

            try
            {
                await _propostaService.AddPropostaAsync(proposta);
                EnviarMensagemProposta(proposta);
                EnviarEventoDeStatus(cliente.Id, "Sucesso");
            }
            catch (Exception ex)
            {
                // Log de erro
                Console.WriteLine($"Erro ao processar proposta: {ex.Message}");
                EnviarEventoDeStatus(cliente.Id, "Falha");
            }
        }

        private void EnviarEventoDeStatus(int clienteId, string status)
        {
            var statusMessage = new { ClienteId = clienteId, Status = status };
            var statusJson = JsonSerializer.Serialize(statusMessage);
            var body = Encoding.UTF8.GetBytes(statusJson);

            _channel.BasicPublish(exchange: "statusExchange", routingKey: "statusRoutingKey", basicProperties: null, body: body);
        }

        private void EnviarMensagemProposta(PropostaDeCredito proposta)
        {
            var propostaJson = JsonSerializer.Serialize(proposta);
            var body = Encoding.UTF8.GetBytes(propostaJson);

            _channel.BasicPublish(exchange: "propostaExchange", routingKey: "propostaRoutingKey", basicProperties: null, body: body);
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