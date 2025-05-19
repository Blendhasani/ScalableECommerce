using Microsoft.EntityFrameworkCore.Metadata;
using ProductService.Application.Product.DTO;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductService.Application.Messaging
{
	public class EventBusPublisher
	{
		public void PublishProductDeleted(int productId)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using var connection = factory.CreateConnection();
			using var channel = connection.CreateModel();

			channel.QueueDeclare(queue: "product-deleted", durable: false, exclusive: false, autoDelete: false, arguments: null);

			var message = $"Product deleted: {productId} at {DateTime.UtcNow}";
			var body = Encoding.UTF8.GetBytes(message);

			channel.BasicPublish(exchange: "", routingKey: "product-deleted", basicProperties: null, body: body);

			Console.WriteLine($"[x] Sent: {message}");
		}
	}


}
