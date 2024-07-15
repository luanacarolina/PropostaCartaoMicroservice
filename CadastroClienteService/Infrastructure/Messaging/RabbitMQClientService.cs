using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CadastroClienteService.Infrastructure.Messaging
{
    public class RabbitMQClientService : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        
        public RabbitMQClientService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "clienteExchange", type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: "clienteQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: "clienteQueue", exchange: "clienteExchange", routingKey: "clienteRoutingKey");
        }

        public void SendMessage(object message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(exchange: "clienteExchange", routingKey: "clienteRoutingKey", basicProperties: null, body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}