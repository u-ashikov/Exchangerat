namespace Exchangerat.Models.Identity
{
    using Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserInputModel
    {
        [Required]
        [MinLength(DataConstants.UsernameMinLength)]
        [MaxLength(DataConstants.UsernameMaxLength)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(DataConstants.UserEmailMinLength)]
        [MaxLength(DataConstants.UserEmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MinLength(DataConstants.PasswordMinLength)]
        [MaxLength(DataConstants.PasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [MinLength(DataConstants.UserFirstNameMinLength)]
        [MaxLength(DataConstants.UserFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(DataConstants.UserLastNameMinLength)]
        [MaxLength(DataConstants.UserLastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MinLength(DataConstants.UserAddressMinLength)]
        [MaxLength(DataConstants.UserAddressMaxLength)]
        public string Address { get; set; }
    }
}
