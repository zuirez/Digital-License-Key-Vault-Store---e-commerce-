using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using Digital_License___Key_Vault_Store.Data;

namespace Digital_License___Key_Vault_Store.Models.Validation
{
    public class UniqueEmailAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string email || string.IsNullOrWhiteSpace(email))
            {
                return ValidationResult.Success;
            }

            var db=validationContext.GetService(typeof(KeyVaultDbContext)) as KeyVaultDbContext;
            if (db == null)
            {
                return ValidationResult.Success;
            }

            var normalizedEmail = email.Trim().ToLower();
            bool exists = db.Users.Any(u => u.Email.ToLower() == normalizedEmail);

            if (exists)
            {
                return new ValidationResult("This Email address is already registered.");
            }

            return ValidationResult.Success;
        }
    }
}
