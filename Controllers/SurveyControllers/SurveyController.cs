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
            var surveys = await context.Surveys
                .Where(s => s.Id != 0)
                .Include(q => q.Questions)
                .ThenInclude(a => a.OferedAnswers)
                .ToListAsync();

            if (surveys != null)
            {
                IList<SurveyViewModel> surveysList = new List<SurveyViewModel>();
                foreach (var survey in surveys)
                {
                    List<QuestionViewModel> questionList = new List<QuestionViewModel>();
                    foreach (var question in survey.Questions)
                    {
                        List<OferedAnswerViewModel> oferedAnswerList = new List<OferedAnswerViewModel>();
                        foreach (var answer in question.OferedAnswers)
                        {
                            OferedAnswerViewModel oferedAnswerView = new OferedAnswerViewModel
                            {
                                Text = answer.Text
                            };
                            oferedAnswerList.Add(oferedAnswerView);
                        }

                        QuestionViewModel questionView = new QuestionViewModel
                        {
                            Text = question.Text,
                            OferedAnswers = oferedAnswerList
                        };
                        questionList.Add(questionView);
                    }

                    SurveyViewModel surveyView = new SurveyViewModel
                    {
                        Name = survey.Name,
                        Description = survey.Description,
                        Questions = questionList
                    };
                    surveysList.Add(surveyView);
                }
                return Ok(surveysList);
            }
            return Ok(null);
        }

        [HttpGet]
        public async Task<IActionResult> GetSurvey(string name)
        {
            var survey = await context.Surveys
                .Where(s => s.Name == name)
                .Include(q => q.Questions)
                .ThenInclude(a => a.OferedAnswers)
                .ToListAsync();

            if (survey != null)
            {
                List<QuestionViewModel> questionList = new List<QuestionViewModel>();
                foreach (var question in survey[0].Questions)
                {
                    List<OferedAnswerViewModel> oferedAnswersList = new List<OferedAnswerViewModel>();
                    foreach (var oferedAnswer in question.OferedAnswers)
                    {
                        OferedAnswerViewModel oferedAnswerView = new OferedAnswerViewModel
                        {
                            Text = oferedAnswer.Text
                        };
                        oferedAnswersList.Add(oferedAnswerView);
                    }

                    QuestionViewModel questionView = new QuestionViewModel
                    {
                        Text = question.Text,
                        OferedAnswers = oferedAnswersList
                    };
                    questionList.Add(questionView);
                }

                SurveyViewModel surveyView = new SurveyViewModel
                {
                    Name = survey[0].Name,
                    Description = survey[0].Description,
                    Questions = questionList
                };
                return Ok(surveyView);
            }
            return Ok(null);

        }

        [HttpPost]
        public IActionResult CreateSurvey([FromBody] SurveyViewModel model)
        {
            Survey surveyModel = null;
            if (model != null)
            {
                var currentUserId = 123; //int.Parse(userManager.GetUserId(this.User));  //********* Odkomentować !!! **************************************************************
                var createDateTime = DateTime.Now;
                IList<Question> questions = new List<Question>();
                foreach (var question in model.Questions)
                {
                    IList<OferedAnswer> oferedAnswers = new List<OferedAnswer>();
                    foreach (var answer in question.OferedAnswers)
                    {
                        OferedAnswer oferedAnswerModel = new OferedAnswer
                        {
                            UserCreateId = currentUserId,
                            CreateDate = createDateTime,
                            Text = answer.Text
                        };
                        oferedAnswers.Add(oferedAnswerModel);
                    }

                    Question questionModel = new Question
                    {
                        UserCreateId = currentUserId,
                        CreateDate = createDateTime,
                        Text = question.Text,
                        OferedAnswers = oferedAnswers
                    };
                    questions.Add(questionModel);
                }

                surveyModel = new Survey
                {
                    UserCreateId = currentUserId,
                    CreateDate = createDateTime,
                    Name = model.Name,
                    Description = model.Description,
                    Questions = questions
                };
            }

            try
            {
                if (surveyModel != null)
                {
                    context.Surveys.Add(surveyModel);
                    context.SaveChanges();
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }

        [HttpPost("UserResponse")]
        public IActionResult UserResponse(string model)
        {
            return Ok(model);
        }
    }
}
