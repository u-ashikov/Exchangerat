namespace Exchangerat.Data.Models
{
    using Common.Constants;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        public int Id { get; set; }

        public int SenderAccountId { get; set; }
        public virtual ExchangeAccount SenderAccount { get; set; }

        public int ReceiverAccountId { get; set; }
        public virtual ExchangeAccount ReceiverAccount { get; set; }

        [Range(default, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [MinLength(DataConstants.TransactionDescriptionMinLength)]
        [MaxLength(DataConstants.TransactionDescriptionMaxLength)]
        public string Description { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
