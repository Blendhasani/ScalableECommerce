using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Product.DTO
{
	public class ProductDeletedEvent
	{
		public int ProductId { get; set; }
		public DateTime DeletedAt { get; set; }
	}

}
