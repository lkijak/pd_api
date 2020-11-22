using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Models.Survey
{
    public class QuestionModel : BaseModel
    {
        public string Text { get; set; }

        public int SurveyId { get; set; }
        public SurveyModel Survey { get; set; }

        public ICollection<AnswerModel> Answers { get; set; }
        public ICollection<ResponseModel> Responses { get; set; }
    }
}
