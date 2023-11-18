using Microsoft.EntityFrameworkCore;
using TaskLogix.Models;

namespace TaskLogix.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<UserCourse>().HasKey(u => new { u.UserId, u.CourseId});
        }
        public DbSet<User> Users {get;set;}
        public DbSet<Course> Courses {get;set;}

        public DbSet<UserCourse> UserCourses {get;set;}

    }
}