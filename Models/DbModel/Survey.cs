using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.DbModel
{
    public class Survey : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Question> Questions { get; set; }
    }

    public class Question : BaseModel
    {
        [Required]
        public string Text { get; set; }

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        public ICollection<OferedAnswer> OferedAnswers { get; set; }
    }

    public class OferedAnswer : BaseModel
    {
        public string Text { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
