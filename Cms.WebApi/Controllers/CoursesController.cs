using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cms.Data.Repository;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Cms.WebApi.DTOs;

namespace Cms.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoursesController: ControllerBase
    {

        private readonly ICmsRepository cmsRepository;

        private readonly IMapper mapper;

        public CoursesController(ICmsRepository cmsRepository,IMapper thismapper)
        {
            this.cmsRepository = cmsRepository;
            this.mapper = thismapper;
        }
        
        [HttpGet("GetUserName")]
        public string GetUserName(string Name)
        {
            return $"Current User Name is  => {Name}";
        }

        [HttpGet("GetIEnumCourse")]
        public IEnumerable<Course> GetIEnumCourse()
        {
            return cmsRepository.GetAllCourses();
        }
        
         [HttpGet("GetIEnumDtoCourse")]
        public IEnumerable<CourseDto> GetIEnumDtoCourse()
        {
           return mapper.Map<CourseDto[]>(cmsRepository.GetAllCourses()).ToList();
        }

        [HttpGet("GetIActionDtoCourse")]
        public IActionResult GetIActionDtoCourse()
        {
          return Ok(mapper.Map<CourseDto[]>(cmsRepository.GetAllCourses()));

        }
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>>GetAllCourse()
        {
            return mapper.Map<CourseDto[]>(cmsRepository.GetAllCourses()).ToList();
        }
        [HttpPost]
        public ActionResult<CourseDto> AddCourse(CourseDto newCourse)
        {
            try
            {                
                    newCourse = mapper.Map<CourseDto>(cmsRepository.AddCourse(mapper.Map<Course>(newCourse)));
                    return newCourse;
            }   
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("{courseId}")]
        public ActionResult<CourseDto> GetCourse(int courseId)
        {
            try
            {
                if(!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                return mapper.Map<CourseDto>(cmsRepository.GetCourse(courseId));                
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

         [HttpPut("{courseId}")]
        public ActionResult<CourseDto> UpdateCourse(int courseId, CourseDto course)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Course updatedCourse = mapper.Map<Course>(course);
                updatedCourse = cmsRepository.UpdateCourse(courseId, updatedCourse);
                var result = mapper.Map<CourseDto>(updatedCourse);

                return result;
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{courseId}")]
        public ActionResult<CourseDto> DeleteCourse(int courseId)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Course course = cmsRepository.DeleteCourse(courseId);

                if(course == null)
                    return BadRequest();
                    
                var result = mapper.Map<CourseDto>(course);
                return result;
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET ../courses/1/students
        [HttpGet("{courseId}/students")]
        public ActionResult<IEnumerable<StudentDto>> GetStudents(int courseId)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                IEnumerable<Student> students = cmsRepository.GetStudents(courseId);
                var result = mapper.Map<StudentDto[]>(students);
                return result;
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST ../courses/1/students
        [HttpPost("{courseId}/students")]
        public ActionResult<StudentDto> AddStudent(int courseId, StudentDto student)
        {
            try
            {
                if (!cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Student newStudent = mapper.Map<Student>(student);

                // Assign course
                Course course = cmsRepository.GetCourse(courseId);
                newStudent.Course = course;

                newStudent = cmsRepository.AddStudent(newStudent);
                var result = mapper.Map<StudentDto>(newStudent);

                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }

}