using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using User.Application;
using User.Domain.Models;
using User.Domain.Exceptions;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        protected IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LoginRequest loginRequest)
        {
            try
            {
                await _registerService.RegisterAsync(loginRequest.Username,loginRequest.Password);
                return Ok(new { message = "User Registered" });
            }
            catch (PasswordValidationException e)
            {
                return BadRequest(new { message = e.Message });
            }
            catch(UserAlreadyExistsException e)
            {
                return BadRequest(new { message = e.Message });
            }
            
        }
    }
}
