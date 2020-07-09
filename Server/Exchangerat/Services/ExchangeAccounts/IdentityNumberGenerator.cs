namespace Exchangerat.Services.ExchangeAccounts
{
    using System;
    using System.Security.Cryptography;

    public class IdentityNumberGenerator : IIdentityNumberGenerator
    {
        public string Generate()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            var random = BitConverter.ToUInt32(bytes, 0) % 1000000;

            return $"EXRT{string.Format("{0:D6}", random)}";
        }
    }
}
