using System;
using System.Collections.Generic;

namespace OrderService.Infrastructure.Models;

public partial class PaymentDetails
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal? PaidAmount { get; set; }

    public DateTime? PaidAt { get; set; }

    public virtual Orders Order { get; set; } = null!;
}
