using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Product.Commands
{
	public class AddProductCommand
	{
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public decimal Price { get; set; }
		public int Stock{ get; set; }
		public int CategoryId{ get; set; }
	}
}
