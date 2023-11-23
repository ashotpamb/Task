using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using TaskLogix.Models;

namespace TaskLogix.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCourse>().HasKey(u => new { u.UserId, u.CourseId });
            modelBuilder.Entity<ModelFiles>().HasKey(f => new { f.FileID });
            modelBuilder.Entity<ModelFiles>().HasOne(f => f.File).WithMany().HasForeignKey(f => f.FileID);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<ModelFiles> ModelFiles { get; set; }
    }
}