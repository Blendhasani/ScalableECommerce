using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.Infrastructure.Models;

public partial class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NotificationTypes> NotificationTypes { get; set; }

    public virtual DbSet<Notifications> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0774B9CDF2");

            entity.Property(e => e.TypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Notifications>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0727373676");

            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Type).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Notifications_Types");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
