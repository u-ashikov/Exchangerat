namespace Exchangerat.Admin.Services.Implementations.Identity
{
    using Exchangerat.Admin.Services.Contracts;

    public class CurrentTokenService : ICurrentTokenService
    {
        private string currentToken;

        public string Get() => this.currentToken;

        public void Set(string token) => this.currentToken = token;
    }
}
