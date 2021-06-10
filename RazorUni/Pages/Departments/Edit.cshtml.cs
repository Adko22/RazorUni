using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorUni.Data;
using RazorUni.Models;

namespace RazorUni.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly RazorUni.Data.RazorUniContext _context;

        public EditModel(RazorUni.Data.RazorUniContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; }
        public SelectList InstructorNameSL { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (Department == null)
            {
                return NotFound();
            }

            InstructorNameSL = new SelectList(_context.Instructors,
               "ID", "FirstMidName");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var departmentToUpdate = await _context.Departments
                .Include(i => i.Administrator)
                .FirstOrDefaultAsync(i => i.DepartmentID == id);

            if(departmentToUpdate==null)
            {
                return HandleDeletedDepartment();
            }

            _context.Entry(departmentToUpdate)
                .Property("RowVersion").OriginalValue = Department.RowVersion;
           
            if(await TryUpdateModelAsync<Department>(
                departmentToUpdate,
                "Departmnet", 
                s=>s.Name,s=>s.StartDate,s=>s.Budget,s=>s.InstructorID))
            {

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }

                catch(DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save." +
                            "The department was deleted by another user.");
                        return Page();
                    }

                    var dbValues = (Department)databaseEntry.ToObject();
                    await setDbErrorMessage(dbValues, clientValues, _context);

                    Department.RowVersion = (byte[])dbValues.RowVersion;
                    ModelState.Remove("Department.RowVersion");
                }
            }

            InstructorNameSL = new SelectList(_context.Instructors,
          "ID", "FullName", departmentToUpdate.InstructorID);

            return Page();
        }  

        private IActionResult HandleDeletedDepartment()
        {
            var deletedDepartment = new Department();

            ModelState.AddModelError(string.Empty,
                "Unable to save. The department was deleted by another user. ");
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName", Department.InstructorID);
            return Page();
        }

        private async Task setDbErrorMessage(Department dbValues,
            Department clientValues, RazorUniContext context)
        {

            if(dbValues.Name!=clientValues.Name)
            {
                ModelState.AddModelError("Departmnet.Name",
                    $"Current value: {dbValues.Name}");
            }
            if (dbValues.Budget != clientValues.Budget)
            {
                ModelState.AddModelError("Department.Budget",
                    $"Current value: {dbValues.Budget:c}");
            }
            if (dbValues.StartDate != clientValues.StartDate)
            {
                ModelState.AddModelError("Department.StartDate",
                    $"Current value: {dbValues.StartDate:d}");
            }
            if(dbValues.InstructorID!=clientValues.InstructorID)
            {
                Instructor dbInstructor = await _context.Instructors
                    .FindAsync(dbValues.InstructorID);
                ModelState.AddModelError("Department.InstructorID",
                    $"Current value: {dbInstructor?.FullName}");
            }

            ModelState.AddModelError(string.Empty,
              "The record you attempted to edit "
            + "was modified by another user after you. The "
            + "edit operation was canceled and the current values in the database "
            + "have been displayed. If you still want to edit this record, click "
            + "the Save button again.");


        }
    }
}
