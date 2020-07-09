namespace Exchangerat.Services.Identity
{
    public interface ICurrentUserService
    {
        string Id { get; }

        bool IsAdmin { get; }
    }
}
