namespace Exchangerat.Clients.Models.ExchangeAccounts
{
    using System;

    public class ClientExchangeAccountOutputModel : ClientExchangeAccountBaseInfoOutputModel
    {
        public string Type { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
