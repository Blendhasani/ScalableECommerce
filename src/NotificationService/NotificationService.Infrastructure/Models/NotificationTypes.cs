using System;
using System.Collections.Generic;

namespace NotificationService.Infrastructure.Models;

public partial class NotificationTypes
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
}
