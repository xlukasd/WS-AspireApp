using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace WS_AspireApp.MessageProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConnection _connection;
        private IModel _channel;

        private EventingBasicConsumer _consumer;

        public Worker(ILogger<Worker> logger, IConnection connection)
        {
            _logger = logger;
            _connection = connection;           
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("basic", "direct");
            _channel.QueueDeclare("basic-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind("basic-queue", "basic", "basicevent");

            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += ReceivedMessage;

            _channel.BasicConsume(queue: "basic-queue", autoAck: true, consumer: _consumer);

            return base.StartAsync(cancellationToken);
        }

        private void ReceivedMessage(object? sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received message from RabbitMQ: {message}");
            File.AppendAllLines(@"C:\Users\lukas.durovsky\Desktop\temp\_logs\aspire.txt", [message]);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Received -= ReceivedMessage;
            _channel?.Close();
            _connection?.Close();

            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
