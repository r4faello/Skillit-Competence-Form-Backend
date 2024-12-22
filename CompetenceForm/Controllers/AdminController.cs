using CompetenceForm.DTOs;
using CompetenceForm.Models;
using CompetenceForm.Queries;
using CompetenceForm.Repositories._Queries;
using CompetenceForm.Services.CompetenceService;
using CompetenceForm.Services.UserService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompetenceForm.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IUserService _userService;
        private IMediator _mediator;
        public AdminController(IUserService userService, IMediator mediator)
        {
            _userService = userService;
            _mediator = mediator;
        }

        private async Task<User?> GetUserAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return null; }

            var userQuery = new UserQuery { IncludeDrafts = true };
            var userGetResult = await _userService.GetUserByIdAsync(userId, userQuery);
            var user = userGetResult.Data;
            return userGetResult.IsSuccess ? user : null;
        }

        [Authorize]
        [HttpGet("surveyResults", Name = "GetSurveyResults")]
        public async Task<ActionResult> GetSurveyResults()
        {
            var user = await GetUserAsync();
            if (user == null || !user.IsAdmin)
            {
                return Unauthorized();
            }

            var query = new GetSurveyResultsQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }


        [Authorize]
        [HttpGet("unfinishedUserCount", Name = "GetUnfinishedUserCount")]
        public async Task<ActionResult> GetUnfinishedUserCount()
        {
            var user = await GetUserAsync();
            if (user == null || !user.IsAdmin)
            {
                return Unauthorized();
            }

            var query = new GetUnfinishedUserCountQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
    }
}
