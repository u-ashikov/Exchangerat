namespace Exchangerat.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class ExchangeAccount
    {
        public int Id { get; set; }

        public string IdentityNumber { get; set; }

        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public decimal Balance { get; set; }

        public int TypeId { get; set; }
        public virtual ExchangeAccountType Type { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public virtual ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();
    }
}
