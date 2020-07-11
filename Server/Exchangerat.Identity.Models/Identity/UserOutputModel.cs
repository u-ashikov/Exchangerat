namespace Exchangerat.Identity.Models.Identity
{
    using Exchangerat.Identity.Data.Models;

    public class UserOutputModel
    {
        public string Username { get; set; }
        public string Token { get; set; }

        public UserOutputModel(User user, string token)
        {
            Username = user.UserName;
            Token = token;
        }
    }
}
