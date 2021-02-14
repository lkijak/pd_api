using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.ViewModel
{
    public class RegistrationViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
