using System;
using System.Collections.Generic;

namespace ProductService.Infrastructure.Models;

public partial class Categories
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Products> Products { get; set; } = new List<Products>();
}
