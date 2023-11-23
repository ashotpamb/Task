using System.ComponentModel.DataAnnotations;
using TaskLogix.Models;

namespace TaskLogix.Dtos
{
    #pragma warning disable CS8618

    public class UserReadDto
    {

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private DateTime _dateOfBirth;

        private DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = value != default ? value : DateTime.Now;
        }
        public string FormattedDateOfBirth => DateOfBirth.ToString("yyyy/mm/dd");

        public string FullName => $"{FirstName} {LastName}";

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public List<CourseReadDto> Courses { get; set; }
        public List<ModelFiles> Files { get; set; }
    }
}