namespace Exchangerat.Clients.Models.Funds
{
    using System.ComponentModel.DataAnnotations;

    using static Exchangerat.Clients.Common.Constants.DataConstants;

    public class FundInputModel
    {
        [Required]
        [MinLength(CreditCardNumberLength)]
        [MaxLength(CreditCardNumberLength)]
        public string CardIdentityNumber { get; set; }

        [Range(FundMinAmount, FundMaxAmount)]
        public decimal Amount { get; set; }

        public int AccountId { get; set; }
    }
}
