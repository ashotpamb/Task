using System.ComponentModel.DataAnnotations;
using TaskLogix.Models;

namespace TaskLogix.Dtos
{
    public class CourseReadDto
    {
        public string CourseName { get; set; }

        public string Description { get; set; }
    }
}