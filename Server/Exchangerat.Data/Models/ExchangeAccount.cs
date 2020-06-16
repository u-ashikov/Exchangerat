namespace Exchangerat.Data.Models
{
    using Common.Constants;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ExchangeAccount
    {
        public int Id { get; set; }

        [Required]
        [MinLength(DataConstants.ExchangeAccountIdentifierMinLength)]
        [MaxLength(DataConstants.ExchangeAccountIdentifierMaxLength)]
        public string IdentityNumber { get; set; }

        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [Range(default, double.MaxValue)]
        public decimal Balance { get; set; }

        public int TypeId { get; set; }
        public virtual ExchangeAccountType Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
