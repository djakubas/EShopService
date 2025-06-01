using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Domain.Repositories;
using User.Application;
using System.Security.Claims;
using User.Domain.Exceptions;
using AutoMapper;
using User.Application.DTO;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeController : ControllerBase
    {
        
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        public MeController(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Employee,Client")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userId, out var userIdGuid))
                {
                    var user = await _usersRepository.GetUserAsync(userIdGuid);
                    var dto = _mapper.Map<UserDto>(user);
                    return Ok(dto);
                }
                else
                {
                    throw new InvalidUserIdException();
                }
                
            }
            catch(Exception e)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = e.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        [HttpPatch]
        [Authorize(Roles = "Administrator,Employee,Client")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UserUpdateDto userUpdateBody)
        {

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (Guid.TryParse(userId, out var userIdGuid))
                {
                    var user = await _usersRepository.GetUserAsync(userIdGuid);
                    _mapper.Map(userUpdateBody, user); //TUTAJ MAPPING NIE DZIALA
                    var result = await _usersRepository.UpdateAsync(user);

                    var userDto = _mapper.Map<UserDto>(user);

                    return Ok(userDto);
                }
                else
                {
                    throw new InvalidUserIdException();
                }
            }
            catch(Exception e)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Detail = e.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }

        }
    }
}
