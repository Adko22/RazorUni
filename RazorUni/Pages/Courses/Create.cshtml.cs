using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorUni.Data;
using RazorUni.Models;

namespace RazorUni.Pages.Courses
{
    public class CreateModel : DepartmentNamePageModel
    {
        private readonly RazorUni.Data.RazorUniContext _context;

        public CreateModel(RazorUni.Data.RazorUniContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateDepartmentsDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyCourse = new Course();

            if(await TryUpdateModelAsync<Course>(
                emptyCourse,
                "course",
            s=>s.CourseID,s=>s.DepartmentID,s=>s.Title, s=>s.Credits))
            {
                _context.Courses.Add(emptyCourse);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateDepartmentsDropDownList(_context, emptyCourse.DepartmentID);
            return Page();
        }
    }
}
