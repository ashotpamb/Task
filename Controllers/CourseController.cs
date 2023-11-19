using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskLogix.Dtos;
using TaskLogix.Models;
using TaskLogix.Repositories;

namespace TaskLogix.Controllers
{
    [ApiController]
    [Route("")]
    public class CourseControler : Controller
    {
        private readonly ICourseReposiotry _repository;
        private readonly IMapper _mapper;

        public CourseControler(ICourseReposiotry courseReposiotry, IMapper mapper)
        {
            _repository = courseReposiotry;
            _mapper = mapper;
        }

        [HttpPost("course-create")]
        public async Task<ActionResult<CourseCreateDto>> CreateCourse([FromBody] CourseCreateDto courseCreateDto)
        {
            if (_repository.CheckCourseExisting(courseCreateDto.CourseName))
            {
                var course = _mapper.Map<Course>(courseCreateDto);
                await _repository.CreateCourse(course);
                _repository.SaveChanges();
                return Ok(_mapper.Map<CourseReadDto>(course));
            }
            return BadRequest("Course already exist");
        }
        [HttpGet("courses")]
        public async Task<ActionResult<CourseReadDto>> GetAllCourses()
        {

            return Ok(_mapper.Map<List<CourseReadDto>>(_repository.GetAllCourses()));
        }
    }
}