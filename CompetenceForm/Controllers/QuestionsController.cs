using CompetenceForm.DTOs;
using CompetenceForm.Models;
using CompetenceForm.Services.CompetenceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompetenceForm.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly ICompetenceService _competenceService;

        public QuestionsController(ICompetenceService competenceService)
        {
            _competenceService = competenceService;
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
    }

}
