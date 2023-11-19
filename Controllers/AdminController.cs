using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskLogix.Dtos;
using TaskLogix.Models;
using TaskLogix.Repositories;

namespace TaskLogix.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public AdminController(IUserRepository userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel userLoginModel)
        {
            var user = _repository.FindUserByEmail(userLoginModel.Email);

            if (user != null && user.Role == Roles.Admin)
            {
                var passwordHasher = new PasswordHasher<string>();

                var result = passwordHasher.VerifyHashedPassword(null, user.Password, userLoginModel.Password);

                var users = _repository.GetUsers();

                var userModel = _mapper.Map<List<UserReadDto>>(users);

                foreach (var item in userModel)
                {
                    var userCourses = users.FirstOrDefault(u => u.ID == item.ID)?.UserCourses;

                    if (userCourses != null)
                    {
                        item.Courses = userCourses
                            // .Where(uc => uc.UserId == item.ID) 
                            .Select(uc => _mapper.Map<CourseReadDto>(uc.Course))
                            .ToList();
                    }
                }
                switch (result)
                {
                    case PasswordVerificationResult.Success:
                        return Ok(new { Token = _repository.GenerateToken(user.ID.ToString()), Model = userModel });
                    case PasswordVerificationResult.Failed:
                        return NotFound("Wrong Password");
                    default:
                        return BadRequest("Invalid input");

                }
            }
            else
            {
                return BadRequest("Invalid credentials or not an admin user");
            }
        }


    }
}