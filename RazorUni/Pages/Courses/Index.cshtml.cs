using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorUni.Data;
using RazorUni.Models;

namespace RazorUni.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly RazorUni.Data.RazorUniContext _context;

        public IndexModel(RazorUni.Data.RazorUniContext context)
        {
            _context = context;
        }

        public IList<Course> Courses { get;set; }

        public async Task OnGetAsync()
        {
            Courses = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
