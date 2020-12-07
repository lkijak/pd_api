using System.Collections.Generic;

namespace pd_api.Models.DbModel
{
    public class Survey : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<SurveyQuestion> Questions { get; set; }
    }

    public class SurveyQuestion : BaseModel
    {
        public string Text { get; set; }

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        public ICollection<SurveyOferedAnswer> OferedAnswers { get; set; }
    }

    public class SurveyOferedAnswer : BaseModel
    {
        public string Text { get; set; }

        public int QuestionId { get; set; }
        public SurveyQuestion Question { get; set; }
    }
}
