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
        private protected ILoginService _loginService;
        

        public LoginController(ILogger<LoginController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        [HttpPost]
        //[EnableCors("allowLocalOrigin")]
        public async Task<IActionResult> Login([FromBody] User.Domain.Models.LoginRequest loginRequest)
        {
            try
            {
                var token = await _loginService.Login(loginRequest.Username, loginRequest.Password);
                return Ok(new { token });
            }
            catch (InvalidCredentialsException e)
            {
                return Unauthorized(new { message = e.Message }); 
            }
        }


        
        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Administrator")]
        [Route("/Admin")]
        public IActionResult AdminPage()
        {
            return Ok(new { message = "Admin Data - Admin only" });
        }
        
        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Administrator,Employee")]
        [Route("/Employee")]
        public IActionResult EmployeePage()
        {
            return Ok(new { message = "Employee Data - Admin and Employee" } );
        }
        
        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Administrator,Employee,Client")]
        [Route("/Client")]
        public IActionResult ClientPage()
        {
            return Ok(new { message = "Client Data - Admin, Employee and Client" });
        }
    }
}
