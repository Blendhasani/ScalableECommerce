using System;
using System.Collections.Generic;

namespace OrderService.Infrastructure.Models;

public partial class OrderItems
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Orders? Order { get; set; }
}
