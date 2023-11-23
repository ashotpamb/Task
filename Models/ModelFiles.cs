using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TaskLogix.Models
{
    #pragma warning disable CS8618
    [Index(nameof(FileID))]
    [Index(nameof(ModelID))]
    public class ModelFiles
    {
        [Key]
        public int ID {get;set;}
        public int FileID { get; set; }
        public File File { get; set; }
        public string ModelName {get;set;}
        public int ModelID {get; set;}
    }
}