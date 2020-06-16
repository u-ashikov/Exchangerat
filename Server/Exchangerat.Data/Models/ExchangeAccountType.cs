namespace Exchangerat.Data.Models
{
    using System.Collections.Generic;

    public class ExchangeAccountType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();
    }
}
