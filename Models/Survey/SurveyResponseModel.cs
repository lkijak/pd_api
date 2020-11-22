using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Models.Survey
{
    public class SurveyResponseModel : BaseModel
    {
        public int SurveyId { get; set; }
        public SurveyModel Survey { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<ResponseModel> Responses { get; set; }
    }
}
