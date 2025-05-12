using System;
using System.Collections.Generic;

namespace ProductService.Infrastructure.Models;

public partial class Products
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CategoryId { get; set; }

    public virtual Categories? Category { get; set; }
}
