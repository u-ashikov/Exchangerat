namespace Exchangerat.Data.Models
{
    using System;

    public class Transaction
    {
        public int Id { get; set; }

        public int SenderAccountId { get; set; }
        public virtual ExchangeAccount SenderAccount { get; set; }

        public int ReceiverAccountId { get; set; }
        public virtual ExchangeAccount ReceiverAccount { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
