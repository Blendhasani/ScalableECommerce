using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Messaging;

namespace OrderService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly EventBusPublisher _publisher;

		public OrderController()
		{
			_publisher = new EventBusPublisher(); // In production: use DI
		}

		[HttpPost]
		public IActionResult PlaceOrder([FromBody] string orderId)
		{
			_publisher.PublishOrderPlaced(orderId); // Send message to RabbitMQ
			return Ok($"Order {orderId} placed and message published.");
		}
	}
}
