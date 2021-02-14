using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pd_api.Models;
using pd_api.Models.DbModel;
using pd_api.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Controllers.SurveyControllers
{
    [Route("[controller]")]
    public class SurveyController : Controller
    {
        private AppDbContext context;
        private UserManager<AppUser> userManager;

        public SurveyController(AppDbContext ctx,
            UserManager<AppUser> userMgr)
        {
            context = ctx;
            userManager = userMgr;
        }

        [HttpGet("UserResponse")]
        public async Task<IActionResult> GetUserResponse(string surveyName, string userName)
        {
            if (string.IsNullOrWhiteSpace(surveyName) && string.IsNullOrWhiteSpace(userName))
            {
                return ValidationProblem(MessageInfo.UserResponse_WrongData);
            }

            AppUser user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }

            var userResponse = await context.UserResponses
                .Include(a => a.UserResponseQuestionsAndAnswers)
                .OrderBy(o => o.CreateDate)
                .LastOrDefaultAsync(s => s.SurveyName == surveyName && s.UserId == user.Id);

            if (userResponse == null )
            {
                return NotFound();
            }

            IList<UserResponseQuestionAndAnswerViewModel> questionsAnswersList =
                new List<UserResponseQuestionAndAnswerViewModel>();
            foreach (var questionAnswer in userResponse.UserResponseQuestionsAndAnswers)
            {
                UserResponseQuestionAndAnswerViewModel qa = new UserResponseQuestionAndAnswerViewModel
                {
                    QuestionText = questionAnswer.QuestionText,
                    AnswerText = questionAnswer.AnswerText,
                    QuestionNo = questionAnswer.QuestionNo
                };
                questionsAnswersList.Add(qa);
            }
            UserResponseViewModel viewModel = new UserResponseViewModel
            {
                SurveyName = userResponse.SurveyName,
                UserName = userResponse.User.UserName,
                DateCompleting = userResponse.CreateDate,
                UserResponseQuestionAndAnswerViewModels = questionsAnswersList
            };
            return Ok(viewModel);
        }

        [HttpGet("UserResponse/List")]
        public async Task<IActionResult> GetUserResponseList(string surveyName, string userName)
        {
            if (string.IsNullOrEmpty(surveyName) && string.IsNullOrEmpty(userName))
            {
                return ValidationProblem(MessageInfo.UserResponse_WrongData);
            }

            AppUser user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }

            IQueryable<UserResponse> userResponses = context.UserResponses
                       .Include(a => a.UserResponseQuestionsAndAnswers)
                       .Where(s => s.SurveyName == surveyName && s.UserId == user.Id)
                       .OrderByDescending(o => o.CreateDate)
                       .Take(3);

            if (!userResponses.Any())
            {
                return NotFound();
            }

            IList<UserResponseViewModel> userResponsesList = new List<UserResponseViewModel>();
            foreach (var userResponse in userResponses)
            {
                IList<UserResponseQuestionAndAnswerViewModel> questionsAnswersList =
                            new List<UserResponseQuestionAndAnswerViewModel>();
                foreach (var questionAnswer in userResponse.UserResponseQuestionsAndAnswers)
                {
                    UserResponseQuestionAndAnswerViewModel qa = new UserResponseQuestionAndAnswerViewModel
                    {
                        QuestionText = questionAnswer.QuestionText,
                        AnswerText = questionAnswer.AnswerText,
                        QuestionNo = questionAnswer.QuestionNo
                    };
                    questionsAnswersList.Add(qa);
                }
                UserResponseViewModel viewModel = new UserResponseViewModel
                {
                    SurveyName = userResponse.SurveyName,
                    UserName = userResponse.User.UserName,
                    DateCompleting = userResponse.CreateDate,
                    UserResponseQuestionAndAnswerViewModels = questionsAnswersList
                };
                userResponsesList.Add(viewModel);
            }
            return Ok(userResponsesList);
        }

        [HttpPost("UserResponse")]
        public async Task<IActionResult> CreateUserResponse([FromBody] UserResponseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            AppUser user = await userManager.FindByNameAsync(viewModel.UserName);
            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = 123;
            var createDateTime = DateTime.Now;

            IList<UserResponseQuestionAndAnswer> questionAnswerList = new List<UserResponseQuestionAndAnswer>();
            foreach (var questionAnswer in viewModel.UserResponseQuestionAndAnswerViewModels)
            {
                UserResponseQuestionAndAnswer userResponseQuestionAndAnswer = new UserResponseQuestionAndAnswer
                {
                    UserCreateId = currentUserId,
                    CreateDate = createDateTime,
                    QuestionText = questionAnswer.QuestionText,
                    AnswerText = questionAnswer.AnswerText,
                    QuestionNo = questionAnswer.QuestionNo
                };
                questionAnswerList.Add(userResponseQuestionAndAnswer);
            }

            UserResponse userResponse = new UserResponse
            {
                UserCreateId = currentUserId,
                CreateDate = createDateTime,
                UserId = currentUserId,
                SurveyName = viewModel.SurveyName,
                User = user,
                UserResponseQuestionsAndAnswers = questionAnswerList
            };

            try
            {
                context.UserResponses.Add(userResponse);
                await context.SaveChangesAsync();
                return Created("/Survey/UserResponse", viewModel);
            }
            catch (Exception)
            {
                throw;
            }  
        }
    }
}
