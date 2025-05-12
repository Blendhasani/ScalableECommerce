using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Product.Abstractions;
using ProductService.Application.Product.Commands;
using ProductService.Application.Product.DTO;

namespace ProductService.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[HttpPost]
		public async Task<IActionResult> AddProduct([FromBody] AddProductCommand command)
		{
			var result = await _productService.AddProductAsync(command);
			return Ok(new { ProductId = result });
		}

		[HttpGet]
		public async Task<ActionResult<List<ProductsDto>>> GetAll()
		{
			var products = await _productService.GetAllProductsAsync();
			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDto>> GetById(int id)
		{
			var product = await _productService.GetByIdAsync(id);
			if (product == null) return NotFound();
			return Ok(product);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] EditProductCommand command)
		{

			var success = await _productService.UpdateAsync(id, command);
			if (!success) return NotFound();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var success = await _productService.DeleteAsync(id);
			if (!success) return NotFound();
			return NoContent();
		}
	}
}
