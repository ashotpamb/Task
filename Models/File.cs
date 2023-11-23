using System.ComponentModel.DataAnnotations;

namespace TaskLogix.Models
{
    public class File
    {
        [Key]
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
    }
}