using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Infrastructure.Models;

public partial class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<OrderItems> OrderItems { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<PaymentDetails> PaymentDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC078B5716CB");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__Order__4CA06362");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC070D55330A");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<PaymentDetails>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentD__3214EC07DAFED86F");

            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidAt).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.PaymentDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentDe__Order__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
