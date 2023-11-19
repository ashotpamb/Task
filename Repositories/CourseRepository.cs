using TaskLogix.Data;
using TaskLogix.Repositories;

namespace TaskLogix.Models
{
    public class CourseRepository:ICourseReposiotry
    {
        private readonly DataContext _context;

        public CourseRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task CreateCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            await _context.AddAsync(course);
        }

        public bool CheckCourseExisting(string courseName)
        {
            return _context.Courses.FirstOrDefault(c => c.CourseName == courseName) == null;
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }

        public List<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }
    }
}