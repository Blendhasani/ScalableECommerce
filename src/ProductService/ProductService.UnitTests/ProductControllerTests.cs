using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.API.Controllers;
using ProductService.Application.Product.Abstractions;
using ProductService.Application.Product.Commands;
using ProductService.Application.Product.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.UnitTests
{
	public class ProductControllerTests
	{
		[Fact]
		public async Task GetAll_ReturnsOkWithExpectedData()
		{
			// Arrange
			var expected = new List<ProductsDto> {
		new ProductsDto {
			Id          = 1,
			Name        = "X",
			Description = "test",
			Price       = 1.5m,
			Stock       = 1,
			CategoryName    = "test"
		}
	};

			var mockService = new Mock<IProductService>();
			mockService
			  .Setup(s => s.GetAllProductsAsync())
			  .ReturnsAsync(expected);

			var controller = new ProductController(mockService.Object);

			// Act
			var actionResult = await controller.GetAll();

			// 1) Assert it returned 200 OK:
			actionResult.Result
						.Should()
						.BeOfType<OkObjectResult>();

			var ok = (OkObjectResult)actionResult.Result!;

			// 2) Assert the payload is exactly our list:
			var actual = ok.Value as List<ProductsDto>;
			actual.Should().NotBeNull();
			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task AddProduct_ReturnsOkWithProductId()
		{
			var command = new AddProductCommand
			{
				Name = "Test",
				Description = "Desc",
				Price = 10,
				Stock = 5,
				CategoryId = 1
			};

			var mockService = new Mock<IProductService>();
			mockService.Setup(s => s.AddProductAsync(command))
					   .ReturnsAsync(42); // example ProductId

			var controller = new ProductController(mockService.Object);

			var result = await controller.AddProduct(command);

			var okResult = result as OkObjectResult;
			okResult.Should().NotBeNull();
			okResult!.Value.Should().BeEquivalentTo(new { ProductId = 42 });
		}

		[Fact]
		public async Task GetById_ProductExists_ReturnsOk()
		{
			var expected = new ProductDto
			{
				Id = 1,
				Name = "P",
				Description = "Sample product description",
				Price = 19.99m,
				Stock = 50,
				Category = new ProductCategoryDto
				{
					CategoryId = 2,
					Name = "Electronics"
				}
			};

			var mockService = new Mock<IProductService>();
			mockService.Setup(s => s.GetByIdAsync(1))
					   .ReturnsAsync(expected);

			var controller = new ProductController(mockService.Object);

			var result = await controller.GetById(1);

			var okResult = result.Result as OkObjectResult;
			okResult.Should().NotBeNull();
			okResult!.Value.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task GetById_ProductDoesNotExist_ReturnsNotFound()
		{
			var mockService = new Mock<IProductService>();
			mockService.Setup(s => s.GetByIdAsync(999))
					   .ReturnsAsync((ProductDto?)null);

			var controller = new ProductController(mockService.Object);

			var result = await controller.GetById(999);

			result.Result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public async Task Update_ProductExists_ReturnsNoContent()
		{
			var mockService = new Mock<IProductService>();
			mockService.Setup(s => s.UpdateAsync(1, It.IsAny<EditProductCommand>()))
					   .ReturnsAsync(true);

			var controller = new ProductController(mockService.Object);

			var result = await controller.Update(1, new EditProductCommand());

			result.Should().BeOfType<NoContentResult>();
		}

		[Fact]
		public async Task Update_ProductNotFound_ReturnsNotFound()
		{
			var mockService = new Mock<IProductService>();
			mockService.Setup(s => s.UpdateAsync(99, It.IsAny<EditProductCommand>()))
					   .ReturnsAsync(false);

			var controller = new ProductController(mockService.Object);

			var result = await controller.Update(99, new EditProductCommand());

			result.Should().BeOfType<NotFoundResult>();
		}

		[Fact]
		public async Task Delete_ProductNotFound_ReturnsNotFound()
		{
			var mockService = new Mock<IProductService>();
			mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

			var controller = new ProductController(mockService.Object);

			var result = await controller.Delete(99);

			result.Should().BeOfType<NotFoundResult>();
		}


		[Fact]
		public async Task Delete_ProductExists_ReturnsNoContent()
		{
			var mockService = new Mock<IProductService>();
			mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

			var controller = new ProductController(mockService.Object);

			var result = await controller.Delete(1);

			result.Should().BeOfType<NoContentResult>();
		}




	}

}
