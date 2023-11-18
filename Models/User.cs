using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TaskLogix.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public List<UserCourse> UserCourses {get;set;}


    }
}