using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Product.DTO
{
	public class ProductDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public decimal Price { get; set; }
		public int Stock { get; set; }
		public ProductCategoryDto? Category { get; set; }
	}

	public class ProductCategoryDto
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
	}
}
