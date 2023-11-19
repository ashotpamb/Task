using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TaskLogix.Models;

namespace TaskLogix.Dtos
{
    public class UserCreateDto
    {
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        // [RegularExpression(@"^\\d{3}\ \d{3}-\d{3}$", ErrorMessage = "Phone number must be in the format 999 999-999")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public Roles Role => Roles.User;
    }
}