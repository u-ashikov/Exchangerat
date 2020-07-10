﻿namespace Exchangerat.Admin.Models.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class LoginFormModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
