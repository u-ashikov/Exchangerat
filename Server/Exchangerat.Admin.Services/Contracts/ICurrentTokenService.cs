namespace Exchangerat.Admin.Services.Contracts
{
    public interface ICurrentTokenService
    {
        string Get();

        void Set(string token);
    }
}
