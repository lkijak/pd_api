using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Models.Survey
{
    public class ResponseModel : BaseModel
    {
        public int SurveyResponseId { get; set; }
        //public SurveyResponseModel SurveyResponse { get; set; }
        public int QuestionId { get; set; }
        public QuestionModel Question { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int AnswerId { get; set; }
        public AnswerModel Answer { get; set; }
    }
}
