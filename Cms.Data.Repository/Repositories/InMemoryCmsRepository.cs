using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Cms.Data.Repository
{
    public class InMemoryCmsRepository : ICmsRepository
    {
       List<Course> courses = null;
       List<Student> students = null;
       public InMemoryCmsRepository()
       {
           courses = new List<Course>();
           students = new List<Student>();
           courses.Add(
               new Course()
               {
                   CourseId = 1,
                   CourseName = "Computer Science",
                   CourseDuration = 4,
                   CourseType = Course_Type.Engineering
               }
           );
           courses.Add(
               new Course()
               {
                   CourseId = 2,
                   CourseName = "Information Technology",
                   CourseDuration = 4,
                   CourseType = Course_Type.Engineering
               }
           );
           courses.Add(
               new Course()
               {
                   CourseId = 3,
                   CourseName = "MBA",
                   CourseDuration = 2,
                   CourseType = Course_Type.Management
               }
           );

        students.Add(new Student{StudentId = 1,FirstName = "Raja", LastName="K", PhoneNumber="9999994535", Address="Tamilnadu", Course= courses.Where(x=> x.CourseId==2).FirstOrDefault()  }); 
        students.Add(new Student{StudentId = 2,FirstName = "Krishna", LastName="V", PhoneNumber="1234455555", Address="Kerala", Course= courses.Where(x=> x.CourseId==1).FirstOrDefault()  }); 

       }

       public IEnumerable<Course> GetAllCourses()
       {
           return courses.ToList();
       }
        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await Task.Run(()=> courses.ToList());
        }
        public Course AddCourse(Course newCourse){
            newCourse.CourseId = (courses.Max(t=> t.CourseId))+1;
            courses.Add(newCourse);
            return newCourse;
        }
        public bool IsCourseExists(int courseId)
        {
            return courses.Any(x=> x.CourseId == courseId);
        }

        // Individual item
        public Course GetCourse(int courseId)
        {
          return  courses.Where(x=> x.CourseId == courseId).SingleOrDefault();
        }
        public Course UpdateCourse(int courseId, Course updcourse)
        {
          var course=  courses.Where(x=> x.CourseId == courseId).SingleOrDefault();
          
          if(course != null)
          {
              course.CourseName = updcourse.CourseName;
              course.CourseDuration = updcourse.CourseDuration;
              course.CourseType = updcourse.CourseType;
          }
          return course;
        }
        public Course DeleteCourse(int courseId){
            var course=  courses.Where(x=> x.CourseId == courseId).SingleOrDefault();
          
          if(course != null)
          {
              courses.Remove(course);
          }
          return course;
        }

        // Association 
        public IEnumerable<Student> GetStudents(int courseId)
        {
            return students.ToList().Where(t=> t.Course.CourseId == courseId).ToList();
        }
        public Student AddStudent(Student student)
        {
           student.StudentId= (students.Max(x=> x.StudentId))+1;
           students.Add(student);
           return student;
        }
    }


}