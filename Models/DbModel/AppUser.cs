using Microsoft.AspNetCore.Identity;
using System;

namespace pd_api.Models.DbModel
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int UserCreateId { get; set; }
        public int? UserModifyId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
