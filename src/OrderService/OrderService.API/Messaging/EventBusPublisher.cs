using RabbitMQ.Client;
using System.Text;

namespace OrderService.Messaging
{
	public class EventBusPublisher
	{
		public void PublishOrderPlaced(string orderId)
		{
			var factory = new ConnectionFactory() { HostName = "localhost" };
			using var connection = factory.CreateConnection();
			using var channel = connection.CreateModel();

			channel.QueueDeclare("order-placed", false, false, false, null);

			var message = $"Order placed: {orderId}";
			var body = Encoding.UTF8.GetBytes(message);

			channel.BasicPublish("", "order-placed", null, body);

			Console.WriteLine($"[x] Sent: {message}");
		}
	}
}
