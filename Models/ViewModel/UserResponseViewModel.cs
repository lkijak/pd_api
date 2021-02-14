using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.ViewModel
{
    public class UserResponseViewModel
    {
        [Required]
        public string SurveyName { get; set; }

        [Required]
        public string UserName { get; set; }
        public DateTime DateCompleting { get; set; }

        [Required, MinLength(1)]
        public IList<UserResponseQuestionAndAnswerViewModel> UserResponseQuestionAndAnswerViewModels { get; set; }
    }

    public class UserResponseQuestionAndAnswerViewModel
    {
        [Required]
        public string QuestionText { get; set; }

        [Required]
        public string AnswerText { get; set; }

        [Required]
        public int QuestionNo { get; set; }
    }
}
