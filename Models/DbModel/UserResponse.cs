using System.Collections.Generic;

namespace pd_api.Models.DbModel
{
    public class UserResponse : BaseModel
    {
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<UserResponseQuestionAndAnswer> UserResponseQuestionsAndAnswers { get; set; }
    }

    public class UserResponseQuestionAndAnswer : BaseModel
    {
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }

        public int UserResponseId { get; set; }
        public UserResponse UserResponse { get; set; }
    }
}
