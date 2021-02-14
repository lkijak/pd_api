using AutoMapper;
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

namespace pd_api.Controllers.UserTaskControllers
{
    [Route("[controller]")]
    public class UserTaskController : Controller
    {
        private AppDbContext context;
        private UserManager<AppUser> userManager;

        public UserTaskController(AppDbContext ctx,
            UserManager<AppUser> userMgr)
        {
            context = ctx;
            userManager = userMgr;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTask(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return ValidationProblem(MessageInfo.User_DidntPassUserName);
            }

            AppUser user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }

            IQueryable<UserTask> userTaskList = context.UserTasks
                .Include(a => a.Subtasks)
                .Where(u => u.User == user && u.IsArchived == false)
                .OrderBy(o => o.CreateDate);
            if (!userTaskList.Any())
            {
                return NotFound();
            }

            IList<UserTaskViewModel> userTaskViewModels = new List<UserTaskViewModel>();
            foreach (var userTask in userTaskList)
            {
                IList<UserSubtaskViewModel> userSubtaskList = new List<UserSubtaskViewModel>();
                foreach (var subtask in userTask.Subtasks)
                {
                    UserSubtaskViewModel subtaskViewModel = new UserSubtaskViewModel
                    {
                        Name = subtask.Name,
                        IsCompleted = subtask.IsCompleted,
                        TaskStartDate = subtask.TaskStartDate,
                        TaskEndDate = subtask.TaskEndDate
                    };
                    userSubtaskList.Add(subtaskViewModel);
                }

                UserTaskViewModel taskViewModel = new UserTaskViewModel
                {
                    Taskid = userTask.Id,
                    Name = userTask.Name,
                    IsCompleted = userTask.IsCompleted,
                    IsArchiwed = userTask.IsArchived,
                    TaskStartDate = userTask.TaskStartDate,
                    TaskEndDate = userTask.TaskEndDate,
                    UserSubtasks = userSubtaskList,
                    UserName = userTask.User.UserName
                };
                userTaskViewModels.Add(taskViewModel);
            }
            return Ok(userTaskViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserTask([FromBody] UserTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            AppUser user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return NotFound();
            }

            var createDate = DateTime.Now;
            var userCreate = 123;

            UserTask userTask = new UserTask
            {
                CreateDate = createDate,
                UserCreateId = userCreate,
                Name = model.Name,
                IsCompleted = false,
                IsArchived = false,
                User = user,
                Subtasks = null
            };

            try
            {
                context.UserTasks.Add(userTask);
                await context.SaveChangesAsync();
                return Created("/UserTask", model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserTask([FromBody] UserTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            AppUser user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return NotFound();
            }

            UserTask userTask = await context.UserTasks
                .Include(i => i.Subtasks)
                .FirstOrDefaultAsync(t => t.Id == model.Taskid);
            if (userTask == null)
            {
                return NotFound();
            }

            IList<UserSubtask> userSubtaskList = new List<UserSubtask>();
            foreach (var subtask in model.UserSubtasks)
            {
                UserSubtask userSubtask = new UserSubtask
                {
                    Name = subtask.Name,
                    IsCompleted = subtask.IsCompleted,
                    TaskStartDate = subtask.TaskStartDate,
                    TaskEndDate = subtask.TaskEndDate
                };
                userSubtaskList.Add(userSubtask);
            }

            userTask.Name = model.Name;
            userTask.IsCompleted = model.IsCompleted;
            userTask.TaskStartDate = model.TaskStartDate;
            userTask.TaskEndDate = model.TaskEndDate;
            userTask.Subtasks = userSubtaskList;

            try
            {
                context.UserTasks.Update(userTask);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("Archiwe")]
        public async Task<IActionResult> TransferToArchiwe(int taskId)
        {
            if (taskId == 0)
            {
                return ValidationProblem(MessageInfo.UserResponse_WrongData);
            }

            UserTask userTask = await context.UserTasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (userTask == null)
            {
                return NotFound();
            }

            try
            {
                userTask.IsArchived = true;
                context.UserTasks.Update(userTask);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
