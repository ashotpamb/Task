using System.Security.Cryptography;
using TaskLogix.Data;
using TaskLogix.Models;

namespace TaskLogix.Seeders
{
    public static class CourseSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                if (!dbContext.Courses.Any())
                {
                    var courses = Enumerable.Range(1, 20).Select(i =>
                        new Course { CourseName = $"Course {i}",
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry" + 
                        "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book." + 
                        "It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum." }
                    );

                    dbContext.Courses.AddRange(courses);
                    dbContext.SaveChanges();
                }
            }
        }

    }
}