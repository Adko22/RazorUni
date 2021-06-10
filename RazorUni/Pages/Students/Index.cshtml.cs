using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorUni.Data;
using RazorUni.Models;

namespace RazorUni.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly RazorUni.Data.RazorUniContext _context;

        public IndexModel(RazorUni.Data.RazorUniContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<Student> Students { get; set; }


        public async Task OnGetAsync(string sortOrder, string SearchString,
            string currentFilter,int? pageIndex)
        {
            CurrentSort = sortOrder;

            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if(SearchString!=null)
            {
                pageIndex = 1;
            }
            else
            {
                SearchString = currentFilter;
            }


            CurrentFilter = SearchString;

            IQueryable<Student> studentsIQ = from s in _context.Students
                                             select s;

            if (!String.IsNullOrEmpty(SearchString))
            {
                studentsIQ = studentsIQ.Where(s => s.LastName.Contains(SearchString)
                                       || s.FirstMidName.Contains(SearchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    studentsIQ = studentsIQ.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsIQ = studentsIQ.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsIQ = studentsIQ.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 4;
            Students = await PaginatedList<Student>.CreateAsync(
               studentsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
