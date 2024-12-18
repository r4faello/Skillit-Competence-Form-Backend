using CompetenceForm.DTOs;
using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;
using CompetenceForm.Services.CompetenceService;
using CompetenceForm.Services.UserService;
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
        private ICompetenceService _competenceService;
        public AdminController(IUserService userService, ICompetenceService competenceService)
        {
            _userService = userService;
            _competenceService = competenceService;
        }

        private async Task<User?> GetUserAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return null; }

            var userQuery = new UserQuery { IncludeDrafts = true };
            var (userGetResult, user) = await _userService.GetUserByIdAsync(userId, userQuery);
            return userGetResult.IsSuccess ? user : null;
        }

        [Authorize]
        [HttpGet("surveyResults", Name = "GetSurveyResults")]
        public async Task<ActionResult> GetSurveyResults()
        {
            var user = await GetUserAsync();
            if (user == null || !user.IsAdmin) { return Unauthorized(); }

            var (result, records) = await _competenceService.SpitSubmittedRecords();

            if (!result.IsSuccess) { return BadRequest(result.Message); }

            return Ok(records);
        }

        [Authorize]
        [HttpGet("unfinishedUserCount", Name = "GetUnfinishedUserCount")]
        public async Task<ActionResult> GetUnfinishedUserCount()
        {
            var user = await GetUserAsync();
            if (user == null || !user.IsAdmin) { return Unauthorized(); }

            var (result, count) = await _competenceService.GetUnfinishedUserCount();

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(count);
        }


    }
}
