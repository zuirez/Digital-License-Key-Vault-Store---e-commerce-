using System;
using System.Collections.Generic;

namespace Digital_License___Key_Vault_Store.Data.Entities;

public partial class KeyAuditLog
{
    public int LogId { get; set; }

    public int KeyId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string IpAddress { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual LicenseKey Key { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
