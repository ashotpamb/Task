using AutoMapper;
using TaskLogix.Dtos;
using TaskLogix.Models;

namespace TaskLogix.Profiles
{
    public class CourseProfile:Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseCreateDto, Course>();
            CreateMap<Course, CourseReadDto>();
        }
    }
}