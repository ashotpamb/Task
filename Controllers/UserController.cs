using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.RateLimiting;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskLogix.Dtos;
using TaskLogix.Models;
using TaskLogix.Repositories;
using TaskLogix.Services;

namespace TaskLogix.Controllers
{
    [ApiController]
    [Route("")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly IEventService _eventService;

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, IMapper mapper, IEventService eventService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
            _eventService = eventService;
        }


        [HttpPost("Register", Name = "User Registration")]
        public async Task<ActionResult<UserCreateDto>> Register([FromBody] UserCreateDto userCreateDto)
        {
            if (_userRepository.FindUserByEmail(userCreateDto.Email) != null) return BadRequest("Email already used");
            var user = _mapper.Map<User>(userCreateDto);

            _userRepository.RegisterUser(user);
            _userRepository.SaveChanges();
            _ = Task.Run(() => _eventService.InvokeEvent(Events.Events.RegisterUser, user));

            return Ok(new { Message = "User registred success", Email = user.Email });
        }


        [HttpGet("UserData")]
        public IActionResult UserData()
        {
            var token = CheckTokenExpired();
            if (token != null)
            {
                if (token.ValidTo < DateTime.UtcNow)
                {
                    return Unauthorized("Unauthorized");
                }
            }
            else if (token == null)
            {
                return BadRequest("Auth token missing");
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userRepository.GetUserById(Convert.ToInt32(currentUserId));

            var userModel = _mapper.Map<UserReadDto>(user);
            userModel.Courses = user.UserCourses
                .Select(uc => _mapper.Map<CourseReadDto>(uc.Course))
                .ToList();
            return Ok(userModel);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginModel userLoginModel)
        {
            var user = _userRepository.FindUserByEmail(userLoginModel.Email);

            if (user == null)
            {
                return NotFound("User not found");
            }
            else if (user.Role != Roles.User)
            {
                return BadRequest("User not found");

            }

            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("User password not set");
            }

            var passwordHasher = new PasswordHasher<string>();

            var result = passwordHasher.VerifyHashedPassword(null, user.Password, userLoginModel.Password);

            var userModel = _mapper.Map<UserReadDto>(user);
            userModel.Courses = user.UserCourses
                .Select(uc => _mapper.Map<CourseReadDto>(uc.Course))
                .ToList();
            switch (result)
            {
                case PasswordVerificationResult.Success:
                    return Ok(new { Token = _userRepository.GenerateToken(user.ID.ToString()) });
                case PasswordVerificationResult.Failed:
                    return NotFound("Wrong Password");
                default:
                    return BadRequest("Invalid input");

            }
        }

        [HttpDelete("delete-course-from-user/{courseId}")]
        public async Task<IActionResult> DeleteCourseFromUSer(int courseId)
        {
            var token = CheckTokenExpired();
            if (token != null)
            {
                if (token.ValidTo < DateTime.UtcNow)
                {
                    return Unauthorized("Unauthorized");
                }
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return BadRequest("User not found");
            }
            try
            {
                _eventService.InvokeEvent(Events.Events.UpdateUser, "test");
                await _userRepository.DeleteCourse(Convert.ToInt32(currentUserId), courseId);
                var user = _userRepository.GetCoursesForUser(Convert.ToInt32(currentUserId));
                UserReadDto userReadDto = _mapper.Map<UserReadDto>(user);
                userReadDto.Courses = user.UserCourses
                    .Select(uc => _mapper.Map<CourseReadDto>(uc.Course))
                    .ToList();
                return Ok(userReadDto);
            }
            catch (Exception)
            {

                return BadRequest("Course not found");
            }

        }

        //If user logged and trying assign course
        [HttpPost("assign-course-to-user/{courseIds}")]
        public async Task<IActionResult> AssignCourseToUser(string courseIds)
        {

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return BadRequest("User not found");
            }

            try
            {
                await _userRepository.AssignCourseToUser(Convert.ToInt32(currentUserId), courseIds);
                _userRepository.SaveChanges();
                var user = _userRepository.GetCoursesForUser(Convert.ToInt32(currentUserId));
                var userReadDto = _mapper.Map<UserReadDto>(user);
                userReadDto.Courses = user.UserCourses.Select(c => _mapper.Map<CourseReadDto>(c.Course)).ToList();
                return Ok(userReadDto);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }


        }
        private JwtSecurityToken CheckTokenExpired()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                var tokenObj = handler.ReadToken(token) as JwtSecurityToken;

                if (tokenObj != null)
                {
                    return tokenObj;
                }
            }

            return null;
        }


    }
}