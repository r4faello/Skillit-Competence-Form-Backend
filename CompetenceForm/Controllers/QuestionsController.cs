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
            // User validation
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return Unauthorized(); }

            var userQuery = new UserQuery { IncludeDrafts = true };
            var (userGetResult, user) = await _userService.GetUserByIdAsync(userId, userQuery);
            if (!userGetResult.IsSuccess || user == null) { return Unauthorized(); }

            var currentCompSetId = await _competenceService.GetCurrentCompetenceSetId();
            var currentCompSetDraft = user.Drafts.FirstOrDefault(d => d.CompetenceSet.Id == currentCompSetId);

            var currentCompSetDraftId = "";
            if(currentCompSetDraft != null)
            {
                currentCompSetDraftId = currentCompSetDraft.Id;
            }

            var (result, response) = await _competenceService.SpitCompetenceSet(user);

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

        [Authorize]
        [HttpPost("deleteDrafts", Name = "DeleteDraft")]
        public async Task<ActionResult> DeleteDraft()
        {
            // User validation
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return Unauthorized(); }

            var userQuery = new UserQuery { IncludeDrafts = true };
            var (userGetResult, user) = await _userService.GetUserByIdAsync(userId, userQuery);
            if (!userGetResult.IsSuccess || user == null) { return Unauthorized(); }

            var result = await _competenceService.DeleteUserDrafts(user);

            if (!result.IsSuccess)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpPost("Seed", Name = "Seed")]
        public async Task<ActionResult> SeedCompetences()
        {
            var result = await _competenceService.Seed(10, (3, 5), (1, 5));

            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return Ok("Competence set has been successfully seeded.");
        }
    
    }

}
