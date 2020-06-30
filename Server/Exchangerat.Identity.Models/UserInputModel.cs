namespace Exchangerat.Identity.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants;
    using static Exchangerat.Identity.Common.Constants.DataConstants;

    public class UserInputModel
    {
        public UserInputModel() { }

        public UserInputModel(string username, string password)
        {
            this.UserName = username;
            this.Password = password;
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(UserEmailMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(UserEmailMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
