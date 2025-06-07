using ProductService.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.UnitTests
{
	public class FakeEventBusPublisher : IEventBusPublisher
	{
		public void PublishProductDeleted(int productId)
		{
			//no op
		}
	}

}
