namespace Exchangerat.Clients.IntegrationTests.Models.Funds
{
    using System;

    public class TestFundOutputModel
    {
        public string CardIdentityNumber { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public string AccountIdentityNumber { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
