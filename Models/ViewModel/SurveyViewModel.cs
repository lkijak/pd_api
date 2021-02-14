using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.ViewModel
{
    public class SurveyViewModel
    {
        public int SurveyId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required, MinLength(1)]
        public IList<SurveyQuestionViewModel> Questions { get; set; }
    }

    public class SurveyQuestionViewModel
    {
        [Required]
        public string Text { get; set; }
        public IList<SurveyOferedAnswerViewModel> OferedAnswers { get; set; }
    }

    public class SurveyOferedAnswerViewModel
    {
        [Required]
        public string Text { get; set; }
    }
}
