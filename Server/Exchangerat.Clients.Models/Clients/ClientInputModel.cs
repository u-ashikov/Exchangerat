namespace Exchangerat.Clients.Models.Clients
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.DataConstants;
    using static Constants.DataConstants;

    public class ClientInputModel
    {
        [Required]
        [Display(Name = "First name")]
        [MinLength(ClientFirstNameMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(ClientFirstNameMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        [MinLength(ClientLastNameMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(ClientLastNameMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string LastName { get; set; }

        [Required]
        [MinLength(ClientAddressMinLength, ErrorMessage = ModelPropertyMinLengthErrorMessage)]
        [MaxLength(ClientAddressMaxLength, ErrorMessage = ModelPropertyMaxLengthErrorMessage)]
        public string Address { get; set; }
    }
}
