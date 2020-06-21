namespace Exchangerat.Models.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class LoginUserInputModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
