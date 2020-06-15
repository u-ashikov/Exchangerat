﻿namespace Exchangerat.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ExchangeAccountType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
