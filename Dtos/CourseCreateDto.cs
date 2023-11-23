using System.ComponentModel.DataAnnotations;
using TaskLogix.Models;

namespace TaskLogix.Dtos
{
    #pragma warning disable CS8618

    public class CourseCreateDto
    {
        [Required(ErrorMessage = "Course name is requires")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Course description is requires")]
        public string Description { get; set; }
    }
}