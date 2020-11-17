﻿using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.Account
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
