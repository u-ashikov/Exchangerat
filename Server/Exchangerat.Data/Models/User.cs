namespace Exchangerat.Data.Models
{
    using Common.Constants;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        [Required]
        [MinLength(DataConstants.UserAddressMinLength)]
        [MaxLength(DataConstants.UserAddressMaxLength)]
        public string Address { get; set; }

        public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();
    }
}
