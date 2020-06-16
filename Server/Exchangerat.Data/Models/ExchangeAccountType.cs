namespace Exchangerat.Data.Models
{
    using Common.Constants;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ExchangeAccountType
    {
        public int Id { get; set; }

        [Required]
        [MinLength(DataConstants.ExchangeAccountTypeNameMinLength)]
        [MaxLength(DataConstants.ExchangeAccountTypeNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(DataConstants.ExchangeAccountTypeDescriptionMinLength)]
        [MaxLength(DataConstants.ExchangeAccountTypeDescriptionMaxLength)]
        public string Description { get; set; }

        public virtual ICollection<ExchangeAccount> ExchangeAccounts { get; set; } = new List<ExchangeAccount>();
    }
}
