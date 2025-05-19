using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Application.Messaging
{
	public class ProductDeletedConsumer
	{
		public void Start()
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using var connection = factory.CreateConnection();
			using var channel = connection.CreateModel();

			channel.QueueDeclare(queue: "product-deleted", durable: false, exclusive: false, autoDelete: false, arguments: null);

			Console.WriteLine(" [*] InventoryService listening for 'product-deleted'...");

			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);

				Console.WriteLine($" [InventoryService] Received: {message}");

				// TODO: parse the message and act accordingly
			};

			channel.BasicConsume(queue: "product-deleted", autoAck: true, consumer: consumer);

			Console.ReadLine(); // Keep the service running
		}
	}

}
