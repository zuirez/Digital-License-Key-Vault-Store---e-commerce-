using System;
using System.Collections.Generic;

namespace Digital_License___Key_Vault_Store.Data.Entities;

public partial class LicenseKey
{
    public int KeyId { get; set; }

    public int ProductId { get; set; }

    public int? OrderItemId { get; set; }

    public int AddedByUserId { get; set; }

    public string EncryptedKey { get; set; } = null!;

    public string KeyHint { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? ReservedUntil { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User AddedByUser { get; set; } = null!;

    public virtual ICollection<KeyAuditLog> KeyAuditLogs { get; set; } = new List<KeyAuditLog>();

    public virtual OrderItem? OrderItem { get; set; }

    public virtual Product Product { get; set; } = null!;
}
