using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Models;

public partial class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Inventory> Inventory { get; set; }

    public virtual DbSet<InventoryLogs> InventoryLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC070B4F6897");

            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<InventoryLogs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC0703764789");

            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
