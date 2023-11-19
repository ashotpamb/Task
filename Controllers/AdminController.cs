using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskLogix.Models;
using TaskLogix.Repositories;

namespace TaskLogix.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserRepository _repository;


        public AdminController(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel userLoginModel)
        {
            var user = _repository.FindUserByEmail(userLoginModel.Email);

            if (user != null && user.Role == Roles.Admin)
            {
                return Ok("Admin login successful");
            }
            else
            {
                return BadRequest("Invalid credentials or not an admin user");
            }
        }


    }
}