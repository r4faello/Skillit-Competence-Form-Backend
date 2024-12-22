using CompetenceForm.Commands;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using CompetenceForm.Queries;
using CompetenceForm.Repositories._Queries;
using CompetenceForm.Services.CompetenceService;
using CompetenceForm.Services.UserService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static CompetenceForm.Repositories.CompetenceRepository;

namespace CompetenceForm.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ICompetenceService _competenceService;
        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public QuestionsController(ICompetenceService competenceService, IUserService userService, IMediator mediator)
        {
            _competenceService = competenceService;
            _userService = userService;
            _mediator = mediator;
        }

        private async Task<User?> GetUserAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return null; }

            var userQuery = new UserQuery { IncludeDrafts = true };
            var userGetResult = await _userService.GetUserByIdAsync(userId, userQuery);
            return userGetResult.IsSuccess ? userGetResult.Data : null;
        }

        [Authorize]
        [HttpGet("", Name = "GetAllQuestions")]
        public async Task<ActionResult<CompetenceSetDto>> GetAllQuestions()
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var query = new GetAllQuestionsQuery(user);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result.Data);
        }


        [Authorize]
        [HttpPost("SaveAnsweredQuestion", Name = "SaveAnsweredQuestion")]
        public async Task<ActionResult> SaveDraft([FromBody] SaveAnsweredQuestionDto details)
        {
            var user = await GetUserAsync();
            if (user == null)
            {
                return Unauthorized();
            }

            var command = new SaveAnsweredQuestionCommand(user, details.CompetenceSetId, details.CompetenceId, details.AnswerId);
            var result = await _mediator.Send(command);

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
            if (user == null)
            {
                return Unauthorized();
            }

            var command = new DeleteDraftCommand(user);
            var result = await _mediator.Send(command);

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
            if (user == null)
            {
                return Unauthorized();
            }

            var command = new FinalizeDraftCommand(user);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }



        [HttpPost("Seed", Name = "Seed")]
        public async Task<ActionResult> SeedCompetences([FromBody] CompetenceSetJson jsonData)
        {
            var command = new SeedCompetencesCommand(jsonData);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return StatusCode(500, result.Message);
            }

            return Ok("Competence set has been successfully seeded.");
        }
    }
}
