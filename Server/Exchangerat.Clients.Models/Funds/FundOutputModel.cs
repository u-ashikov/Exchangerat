namespace Exchangerat.Clients.Models.Funds
{
    using System;

    public class FundOutputModel
    {
        public string CardIdentityNumber { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public string AccountIdentityNumber { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
