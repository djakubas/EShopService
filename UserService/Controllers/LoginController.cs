using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using User.Application;
using User.Domain.Exceptions;
using User.Domain.Models;
namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        protected ILoginService _loginService;

        public LoginController(ILogger<LoginController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        [HttpPost]
        [EnableCors("allowedOrigins")]
        public async Task<IActionResult> Login([FromBody] User.Domain.Models.LoginRequest loginRequest)
        {
            try
            {
                var token = await _loginService.Login(loginRequest.Username, loginRequest.Password);
                return Ok(new { token });
            }
            catch (InvalidCredentialsException e)
            {
                return Unauthorized(e.Message); 
            }
        }

        [HttpGet]
        [Authorize]
        [Authorize(Policy = "Administrator")]
        [Route("/Admin")]
        public IActionResult AdminPage()
        {
            return Ok("Admin Data - Admin only");
        }
        
        [HttpGet]
        [Authorize]
        [Authorize(Policy = "Employee")]
        [Route("/Employee")]
        public IActionResult EmployeePage()
        {
            return Ok("Employee Data - Admin and Employee");
        }
        
        [HttpGet]
        [Authorize]
        [Authorize(Policy = "Client")]

        [Route("/Client")]
        public IActionResult ClientPage()
        {
            return Ok("Client Data - Admin, Employee and Client");
        }
    }
}
