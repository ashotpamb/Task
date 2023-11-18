namespace TaskLogix.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }

        public IList<UserCourse> UserCourses{ get;set;}

        
    }
}