using TaskLogix.Models;

namespace TaskLogix.Repositories
{
    public interface ICourseReposiotry
    {
        public Task CreateCourse(Course course);
        bool CheckCourseExisting(string courseName);
        bool SaveChanges();
    }
}