using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Product.DTO
{
	public class ProductsDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public string CategoryName { get; set; }
	}
}
