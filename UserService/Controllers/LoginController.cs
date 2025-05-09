using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using User.Application.Services;
using User.Domain.Exceptions;
using User.Domain.Models;
using User.Application;
using User.Infrastructure;
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
        [EnableCors("allowAnyOriginAnyHeaderAnyMethod")]
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

        //[HttpPost]
        //[EnableCors("allowAnyOriginAnyHeaderAnyMethod")]
        //public async Task<IActionResult> Register([FromBody] User.Domain.Models.LoginRequest loginRequest)
        //{
        //    return Ok("to do register service");    
        //}
        
        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Administrator")]
        [Route("/Admin")]
        [EnableCors("allowAnyOriginAnyHeaderAnyMethod")]
        public IActionResult AdminPage()
        {
            return Ok("Admin Data - Admin only");
        }
        
        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Administrator,Employee")]
        [Route("/Employee")]
        [EnableCors("allowAnyOriginAnyHeaderAnyMethod")]
        public IActionResult EmployeePage()
        {
            return Ok("Employee Data - Admin and Employee");
        }
        
        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Administrator,Employee,Client")]
        [Route("/Client")]
        [EnableCors("allowAnyOriginAnyHeaderAnyMethod")]
        public IActionResult ClientPage()
        {
            return Ok("Client Data - Admin, Employee and Client");
        }
    }
}
