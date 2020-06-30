namespace Exchangerat.Clients.Models.Transactions
{
    using Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class TransactionInputModel
    {
        [Required]
        public string SenderAccount { get; set; }

        [Required]
        public string ReceiverAccount { get; set; }

        [Range(DataConstants.TransactionMinAmount, DataConstants.TransactionMaxAmount)]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(DataConstants.ExchangeAccountTypeDescriptionMaxLength)]
        public string Description { get; set; }
    }
}
