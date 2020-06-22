namespace Exchangerat.Models.Identity
{
    using System.ComponentModel.DataAnnotations;
    using static Common.Constants.DataConstants;

    public class RegisterUserInputModel
    {
        [Required]
        [MinLength(UsernameMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(UsernameMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(UserEmailMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(UserEmailMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string Email { get; set; }

        [Required]
        [MinLength(PasswordMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(PasswordMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First name")]
        [MinLength(UserFirstNameMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(UserFirstNameMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [MinLength(UserLastNameMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(UserLastNameMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string LastName { get; set; }

        [Required]
        [MinLength(UserAddressMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(UserAddressMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string Address { get; set; }
    }
}
