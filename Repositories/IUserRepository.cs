using TaskLogix.Dtos;
using TaskLogix.Models;

namespace TaskLogix.Repositories
{
    public interface IUserRepository
    {
        bool SaveChanges();
        void RegisterUser(User user);
        User FindUserByEmail(string email);
        string GenerateToken(string userId);
        IEnumerable<User> GetUsers();
        User GetUserById(int Id);

        User GetCoursesForUser(int userId);
        Task AssignCourseToUser(int userId, string courseId);
        Task DeleteCourse(int userId, int courseId);
        Task DeleteAssignedCourse(int userId, Course course);

    }
}