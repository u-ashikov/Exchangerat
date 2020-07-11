namespace Exchangerat.Identity.Models.Identity
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.DataConstants;
    using static Common.Constants.WebConstants;
    using static Constants.DataConstants;

    public class RegisterUserInputModel
    {
        public RegisterUserInputModel() { }

        public RegisterUserInputModel(string username, string password)
        {
            this.UserName = username;
            this.Password = password;
        }

        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(UserEmailMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(UserEmailMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string Email { get; set; }

        [Required]
        [MinLength(PasswordMinLength)]
        [MaxLength(PasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [Display(Name = ModelConstants.ConfirmPassword)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
