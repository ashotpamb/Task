using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskLogix.Dtos;
using TaskLogix.Models;
using TaskLogix.Repositories;

namespace TaskLogix.Controllers
{
    [ApiController]
    [Route("")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("Register", Name = "User Registration")]
        public async Task<ActionResult<UserCreateDto>> Register([FromBody] UserCreateDto userCreateDto)
        {
            if (_userRepository.FindUserByEmail(userCreateDto.Email) != null) return BadRequest("Email already used");

            var user = _mapper.Map<User>(userCreateDto);

            _userRepository.RegisterUser(user);
            _userRepository.SaveChanges();
            return Ok(new { Message = "User registred success", Email = user.Email });
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginModel userLoginModel)
        {
            var user = _userRepository.FindUserByEmail(userLoginModel.Email);

            if (user == null)
            {
                return NotFound("User not found");
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
                    return Ok(new { Token = _userRepository.GenerateToken(user.ID.ToString()), Model = userModel });
                case PasswordVerificationResult.Failed:
                    return NotFound("Wrong Password");
                default:
                    return BadRequest("Invalid input");

            }
        }

        [HttpDelete("delete-course-from-user/{courseId}")]
        public async Task<IActionResult> DeleteCourseFromUSer(int courseId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized();
            }
            await _userRepository.DeleteCourse(Convert.ToInt32(currentUserId), courseId);
            var user = _userRepository.GetCoursesForUser(Convert.ToInt32(currentUserId));
            UserReadDto userReadDto = _mapper.Map<UserReadDto>(user);
            userReadDto.Courses = user.UserCourses
                .Select(uc => _mapper.Map<CourseReadDto>(uc.Course))
                .ToList();
            return Ok(userReadDto);
        }

        //If user logged and trying assign course
        [HttpPost("assign-course-to-user/{courseId}")]
        public async Task<IActionResult> AssignCourseToUser(string courseId)
        {

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            try
            {
                await _userRepository.AssignCourseToUser(Convert.ToInt32(currentUserId), courseId);
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
    }
}