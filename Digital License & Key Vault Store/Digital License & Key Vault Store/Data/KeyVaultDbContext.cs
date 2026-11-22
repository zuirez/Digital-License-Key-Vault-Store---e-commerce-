using System;
using System.Collections.Generic;
using Digital_License___Key_Vault_Store.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Digital_License___Key_Vault_Store.Data;

public partial class KeyVaultDbContext : DbContext
{
    public KeyVaultDbContext()
    {
    }

    public KeyVaultDbContext(DbContextOptions<KeyVaultDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<KeyAuditLog> KeyAuditLogs { get; set; }

    public virtual DbSet<LicenseKey> LicenseKeys { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=KeyVaultDBContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B27EE1FE7");

            entity.HasIndex(e => e.Slug, "UQ__Categori__BC7B5FB64AF3DFBC").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(80);
            entity.Property(e => e.Slug).HasMaxLength(90);
        });

        modelBuilder.Entity<KeyAuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__KeyAudit__5E548648C388757B");

            entity.HasIndex(e => new { e.KeyId, e.Timestamp }, "IX_KeyAuditLogs_KeyId_Timestamp").IsDescending(false, true);

            entity.Property(e => e.Action).HasMaxLength(30);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Key).WithMany(p => p.KeyAuditLogs)
                .HasForeignKey(d => d.KeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KeyAuditL__KeyId__35BCFE0A");

            entity.HasOne(d => d.User).WithMany(p => p.KeyAuditLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KeyAuditL__UserI__36B12243");
        });

        modelBuilder.Entity<LicenseKey>(entity =>
        {
            entity.HasKey(e => e.KeyId).HasName("PK__LicenseK__21F5BE47E54E069E");

            entity.HasIndex(e => new { e.ProductId, e.Status }, "IX_LicenseKeys_Product_Status");

            entity.HasIndex(e => e.ReservedUntil, "IX_LicenseKeys_ReservedUntil").HasFilter("([Status]='Reserved')");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EncryptedKey).HasMaxLength(500);
            entity.Property(e => e.KeyHint).HasMaxLength(10);
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Available");

            entity.HasOne(d => d.AddedByUser).WithMany(p => p.LicenseKeys)
                .HasForeignKey(d => d.AddedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LicenseKe__Added__300424B4");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.LicenseKeys)
                .HasForeignKey(d => d.OrderItemId)
                .HasConstraintName("FK__LicenseKe__Order__2F10007B");

            entity.HasOne(d => d.Product).WithMany(p => p.LicenseKeys)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LicenseKe__Produ__2E1BDC42");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF91DD7B75");

            entity.HasIndex(e => e.UserId, "IX_Orders_UserId");

            entity.HasIndex(e => e.IdempotencyKey, "UQ__Orders__A6D161D8E2B66017").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IdempotencyKey).HasMaxLength(100);
            entity.Property(e => e.PaymentRef).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__UserId__239E4DCF");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED0681A7B2D4DA");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__29572725");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Produ__2A4B4B5E");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CDFF5C47D6");

            entity.HasIndex(e => e.Slug, "UQ__Products__BC7B5FB601EE514D").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ImageUrl).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Slug).HasMaxLength(160);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__1CF15040");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A91B80DAD");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B616025099310").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CAFFF93CD");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053473479ECE").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__145C0A3F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
