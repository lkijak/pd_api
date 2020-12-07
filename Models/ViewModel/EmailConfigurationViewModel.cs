using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.ViewModel
{
    public class EmailConfigurationViewModel
    {
        [Required]
        public string FriendlyName { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool UseDefaultCredential { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public bool EnableSSL { get; set; }

        public string DefaultMessageBody { get; set; }
    }
}
