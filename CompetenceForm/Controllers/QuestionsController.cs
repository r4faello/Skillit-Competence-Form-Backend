using CompetenceForm.DTOs;
using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;
using CompetenceForm.Services.CompetenceService;
using CompetenceForm.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CompetenceForm.Controllers;
using static CompetenceForm.Repositories.CompetenceRepository;

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

        private async Task<User?> GetUserAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return null; }

            var userQuery = new UserQuery { IncludeDrafts = true };
            var (userGetResult, user) = await _userService.GetUserByIdAsync(userId, userQuery);
            return userGetResult.IsSuccess ? user : null;
        }

        [Authorize]
        [HttpGet("", Name = "GetAllQuestions")]
        public async Task<ActionResult<CompetenceSetDto>> GetAllQuestions()
        {
            var user = await GetUserAsync();
            if(user == null) { return Unauthorized(); }

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
            var user = await GetUserAsync();
            if (user == null) { return Unauthorized(); }

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
            var user = await GetUserAsync();
            if (user == null) { return Unauthorized(); }

            var result = await _competenceService.DeleteUserDrafts(user);

            if (!result.IsSuccess)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("finalizeDraft", Name = "FinalizeDraft")]
        public async Task<ActionResult> FinalizeResult()
        {
            var user = await GetUserAsync();
            if(user == null) { return Unauthorized(); }

            var result = await _competenceService.FinalizeDraft(user);

            if (!result.IsSuccess){ return BadRequest(result.Message); }

            return Ok();
        }


        [HttpPost("Seed", Name = "Seed")]
        public async Task<ActionResult> SeedCompetences([FromBody] CompetenceSetJson jsonData)
        {
            var result = await _competenceService.SeedCompetenceSetFromJsonAsync(jsonData);

            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return Ok("Competence set has been successfully seeded.");
        }
    
    }

}
