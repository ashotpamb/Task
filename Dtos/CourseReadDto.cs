using System.ComponentModel.DataAnnotations;
using TaskLogix.Models;

namespace TaskLogix.Dtos
{
    #pragma warning disable CS8618

    public class CourseReadDto
    {

        public int ID { get; set; }
        public string CourseName { get; set; }

        public string Description { get; set; }
    }
}