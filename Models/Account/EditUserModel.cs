using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.Account
{
    public class EditUserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Name { get; set; }
        public string Lastname { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
