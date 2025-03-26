using CityProblems.DataAccess.Entities;
using CityProblems.Models;
using CityProblems.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace CityProblems.Controllers
{
    public class IssueController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IIssueService _issueService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public IssueController(ICategoryService categoryService, IIssueService issueService, IUserService userService, IMessageService messageService)
        {
            _categoryService = categoryService;
            _issueService = issueService;
            _userService = userService;
            _messageService = messageService;
        }

		public async Task<IActionResult> Save([FromBody] IssueViewModel model)
        {
            var user = await GetUser();

            if(user is not null)
            {
                var issueEntity = await _issueService.Create(model.Category, model.Description, model.Latitude, model.Longitude,
                    Convert.FromBase64String(model.Photo), user);

                if (issueEntity is not null)
                {
                    var users = await _userService.GetList();

                    if(issueEntity is not null)
                    {
                        issueEntity.Category = await _categoryService.Get(issueEntity.CategoryId.ToString()) ?? new CategoryEntity();

                        users.ForEach(u =>
                        {
                            if(u.isWorker == 1)
                            {
                                Task.Run(() =>
                                {
                                    _messageService.Send(u, user, issueEntity, true);
                                });
                            }
                        });
                    }

                    return Ok(new {message = "Ok!"});
                }

                return BadRequest(new { message = "Во время сохранения произошла ошибка! Перепроверьте данные." });
            }

            return StatusCode(403);
        }

        public async Task<IActionResult> GetById(string id)
        {
            var user = await GetUser();

            if(user is not null && user.isWorker == 1)
            {
                var issue = await _issueService.Get(id);

                return Json(issue);
            }

            return StatusCode(403);
        }

        [Authorize]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusDto dto)
        {
            var user = await GetUser();

            if(user is not null && user.isWorker == 1)
            {
                var issue = await _issueService.Get(dto.Id);

                if (issue is not null)
                {
                    issue.ExecutionState = (ExecutionState)dto.Status;

                    if ((ExecutionState)dto.Status == ExecutionState.Fixed)
                    {
                        await Task.Run(() =>
                        {
                            _messageService.Send(issue.CreatedBy, user, issue);
                        });

                        var isRemoved = await _issueService.Remove(dto.Id);

                        if (isRemoved)
                        {
                            return Ok();
                        }

                        return BadRequest();
                    }

                    else if ((ExecutionState)dto.Status == ExecutionState.InProgress)
                    {
                        await Task.Run(() =>
                        {
                            _messageService.Send(issue.CreatedBy, user, issue);
                        });
                    }

                    var isSuccess = await _issueService.Update(issue);

                    if (isSuccess)
                    {
                        return Ok();
                    }

                    return BadRequest();
                }

                return BadRequest();
            }

            return StatusCode(403);
		}

        private async Task<UserEntity?> GetUser()
        {
            var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(claimValue is not null)
            {
                var user = await _userService.Get(claimValue);

                return user;
            }

            return null;
        }
    }
}
