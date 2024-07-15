using CartaoCreditoService.Domain.Entities;
using CartaoCreditoService.Domain.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace CartaoCreditoService.Infrastructure.Messaging
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly CartaoDeCreditoService _cartaoService;

        public RabbitMQConsumerService(CartaoDeCreditoService cartaoService)
        {
            _cartaoService = cartaoService;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "propostaExchange", type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: "propostaQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: "propostaQueue", exchange: "propostaExchange", routingKey: "propostaRoutingKey");
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var proposta = JsonSerializer.Deserialize<PropostaDeCredito>(message);

                await ProcessarCartao(proposta);
            };
            _channel.BasicConsume(queue: "propostaQueue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private Task ProcessarCartao(PropostaDeCredito proposta)
        {
            for (int i = 0; i < 2; i++) // Exemplo: emissão de 2 cartões
            {
                var cartao = new CartaoDeCredito
                {
                    ClienteId = proposta.ClienteId,
                    Limite = proposta.Limite,
                    Numero = $"4000 1234 5678 {i:0000}" // Exemplo de geração de número de cartão
                };

                _cartaoService.AddCartao(cartao);

                Console.WriteLine($"Cartão de crédito emitido para o cliente {proposta.ClienteId} com limite {proposta.Limite}");
            }
            // Enviar evento de sucesso ou falha para o serviço de Cadastro de Clientes
            EnviarEventoDeStatus($"Cartão emitido para o cliente {proposta.ClienteId}");

            return Task.CompletedTask;
        }


        private void EnviarEventoDeStatus(string statusMessage)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: "statusExchange", type: ExchangeType.Direct);
            channel.QueueDeclare(queue: "statusQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "statusQueue", exchange: "statusExchange", routingKey: "statusRoutingKey");

            var statusJson = JsonSerializer.Serialize(new { Message = statusMessage });
            var body = Encoding.UTF8.GetBytes(statusJson);
            channel.BasicPublish(exchange: "statusExchange", routingKey: "statusRoutingKey", basicProperties: null, body: body);
        }


        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }

    public class PropostaDeCredito
    {
        public int ClienteId { get; set; }
        public decimal Limite { get; set; }
    }
}