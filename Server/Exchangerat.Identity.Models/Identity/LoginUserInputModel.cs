namespace Exchangerat.Identity.Models.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class LoginUserInputModel
    {
        public LoginUserInputModel() { }

        public LoginUserInputModel(string username, string password)
        {
            this.UserName = username;
            this.Password = password;
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
