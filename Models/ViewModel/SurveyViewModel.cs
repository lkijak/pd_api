using System.Collections.Generic;

namespace pd_api.Models.ViewModel
{
    public class SurveyViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }

    public class QuestionViewModel
    {
        public string Text { get; set; }
        public List<OferedAnswerViewModel> OferedAnswers { get; set; }
    }

    public class OferedAnswerViewModel
    {
        public string Text { get; set; }
    }
}
