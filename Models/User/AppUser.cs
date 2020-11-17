using Microsoft.AspNetCore.Identity;


namespace pd_api.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

    }
}
