using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace pd_api.Models.ViewModel
{
    public class UserResponseViewModel
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "The value must be greater than zero")]
        public int SurveyId { get; set; }

        [Required, MinLength(1)]
        public IList<UserResponseQuestionAndAnswerViewModel> UserResponseQuestionAndAnswerViewModels { get; set; }
    }

    public class UserResponseQuestionAndAnswerViewModel
    {
        [Required]
        public string QuestionText { get; set; }

        [Required]
        public string AnswerText { get; set; }
    }
}
