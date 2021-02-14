using System.Collections.Generic;

namespace pd_api.Models.DbModel
{
    public class UserResponse : BaseModel
    {
        public string SurveyName { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<UserResponseQuestionAndAnswer> UserResponseQuestionsAndAnswers { get; set; }
    }

    public class UserResponseQuestionAndAnswer : BaseModel
    {
        public int QuestionNo { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }

        public int UserResponseId { get; set; }
        public UserResponse UserResponse { get; set; }
    }
}
