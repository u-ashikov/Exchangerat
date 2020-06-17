namespace Exchangerat.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();
    }
}
