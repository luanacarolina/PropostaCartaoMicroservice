using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroClienteService.Infrastructure.Messaging
{
    public class RabbitMQClientService(ConnectionFactory factory)
    {
        private readonly IConnection _connection = factory.CreateConnection();
        private readonly IModel _channel = _connection.CreateModel();

        public RabbitMQClientService() : this(new ConnectionFactory() { HostName = "localhost" })
        {
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