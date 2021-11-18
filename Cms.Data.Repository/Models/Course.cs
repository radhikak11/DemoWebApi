using System.Text;
using System.IO;
using System.Linq;

namespace Cms.Data.Repository
{
    public class Course
    {
        public int CourseId{get;set;}
        public string CourseName {get;set;}
        public int CourseDuration {get;set;}
        public Course_Type CourseType {get;set;}
        }    
        public enum Course_Type
        {
            Engineering=1,
            Medical=2,
            Management=3
        }
   }
    