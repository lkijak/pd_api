using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Models.Survey
{
    public class SurveyModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        ICollection<QuestionModel> Questions { get; set; }
        ICollection<SurveyResponseModel> SurveyResponses { get; set; }
    }
}
