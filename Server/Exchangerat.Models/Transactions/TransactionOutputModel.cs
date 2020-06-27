namespace Exchangerat.Models.Transactions
{
    using System;

    public class TransactionOutputModel
    {
        public string SenderAccountNumber { get; set; }

        public string ReceiverAccountNumber { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
