using System;
using System.Collections.Generic;

namespace InventoryService.Infrastructure.Models;

public partial class Inventory
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int QuantityAvailable { get; set; }

    public DateTime? LastUpdated { get; set; }
}
