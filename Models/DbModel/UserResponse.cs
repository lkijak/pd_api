
namespace pd_api.Models.DbModel
{
    public class UserResponse : BaseModel
    {
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
    }
}
