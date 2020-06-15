namespace Exchangerat.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User : IdentityUser
    {
        [Required]
        public string Address { get; set; }

        public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new HashSet<ExchangeAccount>();
    }
}
