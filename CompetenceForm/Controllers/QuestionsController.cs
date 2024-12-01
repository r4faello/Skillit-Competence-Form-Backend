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
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ICompetenceService _competenceService;
        private readonly IUserService _userService;

        public QuestionsController(ICompetenceService competenceService, IUserService userService)
        {
            _competenceService = competenceService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet("", Name = "GetAllQuestions")]
        public async Task<ActionResult<CompetenceSetDto>> GetAllQuestions()
        {
            var (result, response) = await _competenceService.SpitCompetenceSet();

            if (result.IsSuccess == false)
            {
                return NotFound(result.Message);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost("SaveAnsweredQuestion", Name = "SaveAnsweredQuestion")]
        public async Task<ActionResult> SaveDraft([FromBody] SaveAnsweredQuestionDto details)
        {
            // User validation
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return Unauthorized(); }

            var userQuery = new UserQuery { IncludeDrafts = true };
            var (userGetResult, user) = await _userService.GetUserByIdAsync(userId, userQuery);
            if (!userGetResult.IsSuccess || user == null) { return Unauthorized(); }


            // Saving answer
            var result = await _competenceService.SaveAnsweredQuestion(user, details.CompetenceSetId, details.CompetenceId, details.AnswerId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }
    }

}
