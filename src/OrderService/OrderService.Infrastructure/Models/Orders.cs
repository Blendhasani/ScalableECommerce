using System;
using System.Collections.Generic;

namespace OrderService.Infrastructure.Models;

public partial class Orders
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual ICollection<PaymentDetails> PaymentDetails { get; set; } = new List<PaymentDetails>();
}
