using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorUni.Models.SchoolViewModel
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }

    }
}
