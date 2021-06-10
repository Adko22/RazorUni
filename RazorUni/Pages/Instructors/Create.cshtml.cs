using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorUni.Data;
using RazorUni.Models;

namespace RazorUni.Pages.Instructors
{
    public class CreateModel : InstructorCoursePageModel
    {
        private readonly RazorUni.Data.RazorUniContext _context;

        public CreateModel(RazorUni.Data.RazorUniContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();

            PopulateAssignedCourseData(_context, instructor);
            return Page();
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string[] selectedCourse)
        {
            var newInstructor = new Instructor();
            if(selectedCourse!=null)
            {

                newInstructor.CourseAssignments = new List<CourseAssignment>(); 
                foreach(var course in selectedCourse)
                {
                    var courseToAdd = new CourseAssignment
                    {
                        CourseID = int.Parse(course)
                    };
                    newInstructor.CourseAssignments.Add(courseToAdd);
                }
            }
            if (await TryUpdateModelAsync<Instructor>(
              newInstructor,
              "Instructor",
              i => i.FirstMidName, i => i.LastName,
              i => i.HireDate, i => i.OfficeAssignment))
            {
                _context.Instructors.Add(newInstructor);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            PopulateAssignedCourseData(_context, newInstructor);
            return Page();
        }
    }
}
