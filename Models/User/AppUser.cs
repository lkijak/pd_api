using Microsoft.AspNetCore.Identity;
using pd_api.Models.Survey;
using System;
using System.Collections;
using System.Collections.Generic;

namespace pd_api.Models
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int UserCreateId { get; set; }
        public int UserModifyId { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public ICollection<SurveyResponseModel> SurveyResponses { get; set; }
        public ICollection<ResponseModel> Responses { get; set; }
    }
}
