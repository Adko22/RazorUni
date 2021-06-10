using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUni.Models;
using RazorUni.Data;
using RazorUni.Models.SchoolViewModel;
using System.Security.AccessControl;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace RazorUni.Pages
{
    public class AboutModel : PageModel
    {
        private readonly RazorUniContext _context;

        public AboutModel(RazorUniContext context)
        {
            _context = context;
        }

        public IList<EnrollmentDateGroup> Students { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<EnrollmentDateGroup> data =
                from student in _context.Students
                group student by student.EnrollmentDate into dateGroup
                select new EnrollmentDateGroup()
                {
                    EnrollmentDate = dateGroup.Key,
                    StudentCount = dateGroup.Count()
                };
            Students = await data.AsNoTracking().ToListAsync();
        }


    }
}
