namespace Exchangerat.Clients.Data.Models
{
    using System;

    public class Fund
    {
        public int Id { get; set; }

        public string CardIdentityNumber { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }
        public ExchangeAccount Account { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public DateTime IssuedAt { get; set; }
    }
}
