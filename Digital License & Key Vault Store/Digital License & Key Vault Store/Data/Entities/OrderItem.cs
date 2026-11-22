using System;
using System.Collections.Generic;

namespace Digital_License___Key_Vault_Store.Data.Entities;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual ICollection<LicenseKey> LicenseKeys { get; set; } = new List<LicenseKey>();

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
