using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskLogix.Data;
using TaskLogix.Dtos;
using TaskLogix.Models;
using TaskLogix.Services;

namespace TaskLogix.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly JwtService _jwtService;

        public UserRepository(DataContext dataContext, JwtService jwtService)
        {
            _context = dataContext;
            _jwtService = jwtService;
        }

        public User FindUserByEmail(string email)
        {
            return _context.Users
                    .Include(u => u.UserCourses)
                        .ThenInclude(uc => uc.Course)
                    .FirstOrDefault(u => u.Email == email);
        }

        public void RegisterUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var normalizeAddress = FormatAddress(user.Address);
            user.Address = normalizeAddress;
            var hashPassword = new PasswordHasher<string>();
            string hashedPassword = hashPassword.HashPassword(null, user.Password);
            user.Password = hashedPassword;
            _context.Users.Add(user);
        }

        public static string FormatAddress(string inputAddress)
        {
            string formattedAddress = Regex.Replace(inputAddress, @"\s*\.\s*", " ").Trim().ToUpper();


            var wordReplacements = new Dictionary<string, string>
        {
            {"APARTMENT", "APT" },
            {"AVENUE", "AVE" },
            {"ROAD", "RD" },
            {"STREET", "ST" },
            {"BOULEVARD", "BLVD" },
            {"(NO |#|NUMBER )([0-9]+)(.)", "N $2RD$3" },
            {"(NO |#|NUMBER )", "" }

        };

            foreach (var replacement in wordReplacements)
            {
                formattedAddress = Regex.Replace(formattedAddress, replacement.Key, replacement.Value, RegexOptions.IgnoreCase);
            }

            return formattedAddress;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public string GenerateToken(string userId)
        {
            return _jwtService.GenerateJwtToken(userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users
                .Where(u => u.Role == Roles.User)
                .Include(u => u.UserCourses)
                    .ThenInclude(uc => uc.Course)
                .ToList();
        }

        public User GetUserById(int Id)
        {
            return _context.Users.FirstOrDefault(u => u.ID == Id);
        }

        public async Task AssignCourseToUser(int userId, string courseIds)
        {
            var user = _context.Users.FirstOrDefault(u => u.ID == userId);
            Console.WriteLine(courseIds);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var courseIdArray = courseIds.Split(',').Select(int.Parse).ToArray();

            var courses = _context.Courses.Where(c => courseIdArray.Contains(c.ID)).ToList();

            foreach (var courseId in courseIdArray)
            {
                var course = courses.FirstOrDefault(c => c.ID == courseId);

                if (course == null)
                {
                    throw new Exception($"Course with ID {courseId} not found");
                }

                var existingUserCourse = _context.UserCourses
                                        .FirstOrDefault(uc => uc.UserId == userId && uc.CourseId == courseId);

                if (existingUserCourse != null)
                {
                    throw new Exception($"Course with ID {courseId} already assigned to the user");
                }

                var newUserCourse = new UserCourse
                {
                    UserId = userId,
                    CourseId = courseId
                };

                await _context.UserCourses.AddAsync(newUserCourse);
            }


        }

        public async Task DeleteCourse(int userId, int courseId)
        {
            var user = GetUserById(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var courseForUser = _context.UserCourses.FirstOrDefault(c => c.UserId == userId && c.CourseId == courseId);
            if (courseForUser == null)
            {
                throw new Exception("Course not found");
            }
            _context.UserCourses.Remove(courseForUser);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAssignedCourse(int userId, Course course)
        {
            throw new NotImplementedException();
        }

        public User GetCoursesForUser(int userId)
        {
            var user = GetUserById(userId);

            if (user == null)
            {
                throw new Exception("User not fund");
            }

            var coursesForUser = _context.Users
                    .Include(u => u.UserCourses)
                        .ThenInclude(uc => uc.Course)
                    .FirstOrDefault(u => u.ID == userId);

            return coursesForUser;
        }

        public async Task GetcourseForUser(int userId, int courseId)
        {
            // var user = GetUserById(userId);
            // if (user == null)
            // {
            //     throw new Exception("User not found");
            // }

            // var courseFor
        }
    }
}