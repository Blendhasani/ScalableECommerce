using System;
using System.Collections.Generic;

namespace NotificationService.Infrastructure.Models;

public partial class Notifications
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Message { get; set; }

    public DateTime? SentAt { get; set; }

    public bool? IsRead { get; set; }

    public int? TypeId { get; set; }

    public virtual NotificationTypes? Type { get; set; }
}
