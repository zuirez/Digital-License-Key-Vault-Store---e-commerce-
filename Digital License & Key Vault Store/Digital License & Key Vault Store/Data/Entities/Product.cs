using System;
using System.Collections.Generic;

namespace Digital_License___Key_Vault_Store.Data.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<LicenseKey> LicenseKeys { get; set; } = new List<LicenseKey>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
