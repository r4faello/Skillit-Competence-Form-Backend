using AutoMapper;
using CompetenceForm.DTOs;
using CompetenceForm.Models;
using CompetenceForm.Repositories._Queries;
using CompetenceForm.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompetenceForm.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
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


        [HttpPost("register", Name = "RegisterNewUser")]
        public async Task<ActionResult<string>> RegisterNewUser([FromBody] UserRegisterDto userRegisterDto)
        {
            string username = userRegisterDto.Username;
            string password = userRegisterDto.Password;

            var (result, user) = await _userService.CreateUserAsync(username, password);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            var (res, jwtToken) = await _userService.GenerateJwtAsync(username, password);
            if (!res.IsSuccess)
            {
                return BadRequest(res.Message);
            }

            return Ok(jwtToken);
        }


        [HttpPost("login", Name = "LoginUser")]
        public async Task<ActionResult<string>> LoginUser([FromBody] UserLoginDto userLoginDto)
        {
            string username = userLoginDto.Username;
            string password = userLoginDto.Password;


            var (res, jwtToken) = await _userService.GenerateJwtAsync(username, password);
            if (!res.IsSuccess)
            {
                return BadRequest(res.Message);
            }

            return Ok(jwtToken);
        }

    }
}
