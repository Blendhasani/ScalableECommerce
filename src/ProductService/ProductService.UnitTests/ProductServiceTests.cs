using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using ProductService.Application.Product.Commands;
using ProductService.Application.Product.DTO;
using ProductService.Application.Product.Services;
using ProductService.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.UnitTests
{
	public class ProductServiceTests
	{
		private readonly ProductServiceImpl _service;
		private readonly ProductDbContext _db;
		private readonly IMemoryCache _cache;

		public ProductServiceTests()
		{
			var options = new DbContextOptionsBuilder<ProductDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			_db = new ProductDbContext(options);

			var fakePublisher = new FakeEventBusPublisher();


			_cache = new MemoryCache(new MemoryCacheOptions());

			_service = new ProductServiceImpl(
				_db,
				_cache,
				fakePublisher
			);
		}

		[Fact]
		public async Task AddProductAsync_ReturnsProductId()
		{
			var command = new AddProductCommand { Name = "Test", Description = "D", Price = 10, Stock = 2, CategoryId = 1 };
			var id = await _service.AddProductAsync(command);
			id.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task GetAllProductsAsync_ReturnsListWithCategoryNames()
		{
			_db.Categories.Add(new Categories { Id = 1, Name = "Cat1" });
			_db.Products.Add(new Products { Name = "A", Description = "D", Price = 1, Stock = 1, CategoryId = 1 });
			_db.SaveChanges();

			var result = await _service.GetAllProductsAsync();
			result.Should().ContainSingle();
			result[0].CategoryName.Should().Be("Cat1");
		}

		[Fact]
		public async Task GetByIdAsync_Cached_ReturnsFromCache()
		{
			var expected = new ProductDto
			{
				Id = 1,
				Name = "Cached",
				Description = "Cached product description",
				Price = 29.99m,
				Stock = 20,
				Category = new ProductCategoryDto
				{
					CategoryId = 3,
					Name = "Books"
				}
			};
			_cache.Set("product:1", expected);

			var result = await _service.GetByIdAsync(1);
			result.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task GetByIdAsync_NotCached_ReturnsAndCaches()
		{
			_db.Categories.Add(new Categories { Id = 1, Name = "Cat" });
			_db.Products.Add(new Products { Id = 2, Name = "Uncached", Description = "", Price = 2, Stock = 1, CategoryId = 1 });
			_db.SaveChanges();

			var result = await _service.GetByIdAsync(2);
			result.Should().NotBeNull();

			_cache.TryGetValue("product:2", out var obj).Should().BeTrue();
			var cached = Assert.IsType<ProductDto>(obj);
			cached.Should().BeEquivalentTo(result);
		}

		[Fact]
		public async Task UpdateAsync_Exists_ReturnsTrueAndUpdates()
		{
			_db.Products.Add(new Products { Id = 3, Name = "Old", Description = "", Price = 1, Stock = 1, CategoryId = 1 });
			_db.SaveChanges();

			var result = await _service.UpdateAsync(3, new EditProductCommand { Name = "Updated", Description = "New", Price = 2, Stock = 5, CategoryId = 1 });
			result.Should().BeTrue();
			_db.Products.Find(3)?.Name.Should().Be("Updated");
		}

		[Fact]
		public async Task UpdateAsync_NotFound_ReturnsFalse()
		{
			var result = await _service.UpdateAsync(999, new EditProductCommand());
			result.Should().BeFalse();
		}

		[Fact]
		public async Task DeleteAsync_Exists_ReturnsTrueAndDeletes()
		{
			_db.Products.Add(new Products { Id = 4, Name = "ToDelete" });
			_db.SaveChanges();

			var result = await _service.DeleteAsync(4);
			result.Should().BeTrue();
			_db.Products.Find(4).Should().BeNull();
		}

		[Fact]
		public async Task DeleteAsync_NotFound_ReturnsFalse()
		{
			var result = await _service.DeleteAsync(999);
			result.Should().BeFalse();
		}
	}
}
