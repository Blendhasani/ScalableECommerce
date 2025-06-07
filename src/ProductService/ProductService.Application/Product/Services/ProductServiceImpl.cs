using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProductService.Application.Messaging;
using ProductService.Application.Product.Abstractions;
using ProductService.Application.Product.Commands;
using ProductService.Application.Product.DTO;
using ProductService.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Product.Services
{
	public class ProductServiceImpl : IProductService
	{
		private readonly ProductDbContext _context;
		private readonly IMemoryCache _cache;
		private readonly IEventBusPublisher _publisher;


		public ProductServiceImpl(ProductDbContext context, IMemoryCache cache, IEventBusPublisher publisher)
		{
			_context = context;
			_cache = cache;
			_publisher = publisher;
		}

		public async Task<int> AddProductAsync(AddProductCommand command)
		{
			var product = new ProductService.Infrastructure.Models.Products
			{
				Name = command.Name,
				Description = command.Description,
				Price = command.Price,
				Stock= command.Stock,
				CategoryId = command.CategoryId,
				CreatedAt = DateTime.Now
			};

			_context.Products.Add(product);
			await _context.SaveChangesAsync();
			return product.Id;
		}

		public async Task<List<ProductsDto>> GetAllProductsAsync()
		{
			return await _context.Products
				.Include(p => p.Category)
				.Select(p => new ProductsDto
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					Price = p.Price,
					Stock = p.Stock,
					CategoryName = p.CategoryId != null? p.Category.Name : null
				})
				.ToListAsync();
		}

		public async Task<ProductDto?> GetByIdAsync(int id)
		{
			var cacheKey = $"product:{id}";

			if (_cache.TryGetValue(cacheKey, out ProductDto cachedProduct))
			{
				return cachedProduct;
			}


			var product = await _context.Products
				.Include(x => x.Category)
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();

			if (product == null) return null;

			var result = new ProductDto
			{
				Id = product.Id,
				Name = product.Name,
				Description = product.Description ?? "N/A",
				Price = product.Price,
				Stock = product.Stock,
				Category = product.CategoryId != null ? new ProductCategoryDto
				{
					CategoryId = product.CategoryId ?? 0,
					Name = product.Category != null ? product.Category.Name : "N/A"
				} : null
			};

			_cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
			return result;
		}

		public async Task<bool> UpdateAsync(int id, EditProductCommand command)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return false;

			product.Name = command.Name;
			product.Description = command.Description;
			product.Price = command.Price;
			product.Stock = command.Stock;
			product.CategoryId = command.CategoryId;

			await _context.SaveChangesAsync();
			_cache.Remove($"product:{id}");

			return true;
		}


		public async Task<bool> DeleteAsync(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return false;

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			_cache.Remove($"product:{id}");

			// Publish product deleted message to RabbitMQ
			_publisher.PublishProductDeleted(id);


			return true;
		}


	}
}
