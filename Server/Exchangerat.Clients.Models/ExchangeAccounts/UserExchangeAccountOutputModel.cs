namespace Exchangerat.Clients.Models.ExchangeAccounts
{
    using System;

    public class UserExchangeAccountOutputModel
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }

        public string Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
