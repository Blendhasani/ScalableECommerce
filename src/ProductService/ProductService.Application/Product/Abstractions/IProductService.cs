using ProductService.Application.Product.Commands;
using ProductService.Application.Product.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Product.Abstractions
{
	public interface IProductService
	{
		Task<int> AddProductAsync(AddProductCommand command);
		Task<List<ProductsDto>> GetAllProductsAsync();
		Task<ProductDto?> GetByIdAsync(int id);
		Task<bool> UpdateAsync(int id, EditProductCommand command);
		Task<bool> DeleteAsync(int id);

	}
}
