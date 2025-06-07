

using Microsoft.EntityFrameworkCore;
using ProductService.Application.Product.Abstractions;
using ProductService.Application.Product.Services;
using ProductService.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// 1) Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<ProductDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductService, ProductServiceImpl>();

var app = builder.Build();

// 2) APPLY MIGRATIONS AUTOMATICALLY
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
	db.Database.Migrate();

	// Seed if empty
	if (!db.Products.Any())
	{
		db.Products.AddRange(
			new ProductService.Infrastructure.Models.Products { Name = "Sample A", Description = "First sample", Price = 9.99M, Stock = 100 },
			new ProductService.Infrastructure.Models.Products { Name = "Sample B", Description = "Second sample", Price = 19.99M, Stock = 50 }
			// …add more as desired
		);
		db.SaveChanges();
	}
}

// 3) Configure middleware
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

