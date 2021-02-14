using System.Collections.Generic;

namespace pd_api.Models.ViewModel
{
    public class UserAccountViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool IsLifeCircleFilled { get; set; }
        public IList<string> Role { get; set; }
    }
}
