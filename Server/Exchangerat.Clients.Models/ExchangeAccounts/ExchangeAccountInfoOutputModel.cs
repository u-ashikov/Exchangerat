namespace Exchangerat.Clients.Models.ExchangeAccounts
{
    using System;
    using System.Collections.Generic;
    using Transactions;

    public class ExchangeAccountInfoOutputModel
    {
        public string AccountNumber { get; set; }

        public string AccountType { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<TransactionOutputModel> Transactions { get; set; } = new List<TransactionOutputModel>();
    }
}
