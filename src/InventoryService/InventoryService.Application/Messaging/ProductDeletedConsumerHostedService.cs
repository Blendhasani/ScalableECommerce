using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ProductDeletedConsumerHostedService : BackgroundService
{
	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var factory = new ConnectionFactory() { HostName = "localhost" };
		var connection = factory.CreateConnection();
		var channel = connection.CreateModel();

		channel.QueueDeclare("product-deleted", durable: false, exclusive: false, autoDelete: false, arguments: null);

		var consumer = new EventingBasicConsumer(channel);
		consumer.Received += (model, ea) =>
		{
			var message = Encoding.UTF8.GetString(ea.Body.ToArray());
			Console.WriteLine($"[InventoryService] Received: {message}");
		};

		channel.BasicConsume(queue: "product-deleted", autoAck: true, consumer: consumer);

		return Task.CompletedTask;
	}
}
