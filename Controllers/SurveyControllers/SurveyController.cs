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

        [HttpGet("Surveys")]
        public async Task<IActionResult> GetAllSurvey()
        {
            List<Survey> surveys = await context.Surveys
                .Where(s => s.Id != 0)
                .Include(q => q.Questions)
                .ThenInclude(a => a.OferedAnswers)
                .ToListAsync();

            IList<SurveyViewModel> surveysList = null;
            if (surveys.Any())
            {
                surveysList = new List<SurveyViewModel>();
                foreach (var survey in surveys)
                {
                    IList<SurveyQuestionViewModel> questionList = new List<SurveyQuestionViewModel>();
                    foreach (var question in survey.Questions)
                    {
                        IList<SurveyOferedAnswerViewModel> oferedAnswerList = new List<SurveyOferedAnswerViewModel>();
                        foreach (var answer in question.OferedAnswers)
                        {
                            SurveyOferedAnswerViewModel oferedAnswerView = new SurveyOferedAnswerViewModel
                            {
                                Text = answer.Text
                            };
                            oferedAnswerList.Add(oferedAnswerView);
                        }

                        SurveyQuestionViewModel questionView = new SurveyQuestionViewModel
                        {
                            Text = question.Text,
                            OferedAnswers = oferedAnswerList
                        };
                        questionList.Add(questionView);
                    }

                    SurveyViewModel surveyView = new SurveyViewModel
                    {
                        SurveyId = survey.Id,
                        Name = survey.Name,
                        Description = survey.Description,
                        Questions = questionList
                    };
                    surveysList.Add(surveyView);
                }
            }
            return Ok(surveysList);
        }

        [HttpGet]
        public IActionResult GetSurvey(string name)
        {
            var survey = context.Surveys
                .Include(q => q.Questions)
                .ThenInclude(a => a.OferedAnswers)
                .FirstOrDefault(s => s.Name == name);

            SurveyViewModel surveyView = null;
            if (survey != null)
            {
                IList<SurveyQuestionViewModel> questionList = new List<SurveyQuestionViewModel>();
                foreach (var question in survey.Questions)
                {
                    IList<SurveyOferedAnswerViewModel> oferedAnswersList = new List<SurveyOferedAnswerViewModel>();
                    foreach (var oferedAnswer in question.OferedAnswers)
                    {
                        SurveyOferedAnswerViewModel oferedAnswerView = new SurveyOferedAnswerViewModel
                        {
                            Text = oferedAnswer.Text
                        };
                        oferedAnswersList.Add(oferedAnswerView);
                    }

                    SurveyQuestionViewModel questionView = new SurveyQuestionViewModel
                    {
                        Text = question.Text,
                        OferedAnswers = oferedAnswersList
                    };
                    questionList.Add(questionView);
                }

                surveyView = new SurveyViewModel
                {
                    SurveyId = survey.Id,
                    Name = survey.Name,
                    Description = survey.Description,
                    Questions = questionList
                };
            }
            return Ok(surveyView);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyViewModel model)
        {
            Survey survey = null;
            if (ModelState.IsValid)
            {
                var currentUserId = 123; //int.Parse(userManager.GetUserId(this.User));  //********* Odkomentować !!! **************************************************************
                var createDateTime = DateTime.Now;
                IList<SurveyQuestion> questions = new List<SurveyQuestion>();
                foreach (var question in model.Questions)
                {
                    IList<SurveyOferedAnswer> oferedAnswers = new List<SurveyOferedAnswer>();
                    if (question.OferedAnswers != null)
                    {
                        foreach (var answer in question.OferedAnswers)
                        {
                            SurveyOferedAnswer oferedAnswerModel = new SurveyOferedAnswer
                            {
                                UserCreateId = currentUserId,
                                CreateDate = createDateTime,
                                Text = answer.Text
                            };
                            oferedAnswers.Add(oferedAnswerModel);
                        }
                    }
                    
                    SurveyQuestion questionModel = new SurveyQuestion
                    {
                        UserCreateId = currentUserId,
                        CreateDate = createDateTime,
                        Text = question.Text,
                        OferedAnswers = oferedAnswers
                    };
                    questions.Add(questionModel);
                }

                survey = new Survey
                {
                    UserCreateId = currentUserId,
                    CreateDate = createDateTime,
                    Name = model.Name,
                    Description = model.Description,
                    Questions = questions
                };

                try
                {
                    context.Surveys.Add(survey);
                    await context.SaveChangesAsync();
                    return Created("/Survey/UserResponse", model);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return ValidationProblem(ModelState);
        }

        [HttpGet("UserResponse")]
        public IActionResult GetUserResponse(string surveyName, int userId)
        {
            UserResponse userResponse = context.UserResponses
                .Include(a => a.UserResponseQuestionsAndAnswers)
                .FirstOrDefault(s => s.Survey.Name == surveyName && s.UserId == userId);

            if (userResponse != null)
            {
                IList<UserResponseQuestionAndAnswerViewModel> questionsAnswersList =
                    new List<UserResponseQuestionAndAnswerViewModel>();
                foreach (var questionAnswer in userResponse.UserResponseQuestionsAndAnswers)
                {
                    UserResponseQuestionAndAnswerViewModel qa = new UserResponseQuestionAndAnswerViewModel
                    {
                        QuestionText = questionAnswer.QuestionText,
                        AnswerText = questionAnswer.AnswerText
                    };
                    questionsAnswersList.Add(qa);
                }
                UserResponseViewModel viewModel = new UserResponseViewModel
                {
                    UserResponseQuestionAndAnswerViewModels = questionsAnswersList
                };
            }
            return Ok(userResponse);
        }

        [HttpPost("UserResponse")]
        public async Task<IActionResult> CreateUserResponse([FromBody] UserResponseViewModel viewModel)
        {
            UserResponse userResponse = null;
            if (ModelState.IsValid)
            {
                var currentUserId = 123; //int.Parse(userManager.GetUserId(this.User));  //********* Odkomentować !!! **************************************************************
                var createDateTime = DateTime.Now;

                IList<UserResponseQuestionAndAnswer> questionAnswerList = new List<UserResponseQuestionAndAnswer>();
                foreach (var questionAnswer in viewModel.UserResponseQuestionAndAnswerViewModels)
                {
                    UserResponseQuestionAndAnswer userResponseQuestionAndAnswer = new UserResponseQuestionAndAnswer
                    {
                        UserCreateId = currentUserId,
                        CreateDate = createDateTime,
                        QuestionText = questionAnswer.QuestionText,
                        AnswerText = questionAnswer.AnswerText
                    };
                    questionAnswerList.Add(userResponseQuestionAndAnswer);
                }

                userResponse = new UserResponse
                {
                    UserCreateId = currentUserId,
                    CreateDate = createDateTime,
                    UserId = currentUserId,
                    SurveyId = viewModel.SurveyId,
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
            return ValidationProblem(ModelState);
        }
    }
}
