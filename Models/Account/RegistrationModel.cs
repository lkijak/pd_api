using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.Account
{
    public class RegistrationModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Compare("Email")]
        public string ConfirmEmail { get; set; }

    }
}
