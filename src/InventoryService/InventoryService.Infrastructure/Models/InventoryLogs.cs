using System;
using System.Collections.Generic;

namespace InventoryService.Infrastructure.Models;

public partial class InventoryLogs
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? Change { get; set; }

    public string? Reason { get; set; }

    public DateTime? Timestamp { get; set; }
}
