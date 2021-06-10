using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorUni.Models;

namespace RazorUni.Data
{
    public class RazorUniContext : DbContext
    {
        public RazorUniContext (DbContextOptions<RazorUniContext> options)
            : base(options)
        {
        }

        public DbSet<RazorUni.Models.Student> Students{ get; set; }
        public DbSet<RazorUni.Models.Enrollment> Enrollments { get; set; }
        public DbSet<RazorUni.Models.Course> Courses { get; set; }
        public DbSet<RazorUni.Models.Department> Departments { get; set; }
        public DbSet<RazorUni.Models.Instructor> Instructors { get; set; }
        public DbSet<RazorUni.Models.OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<RazorUni.Models.CourseAssignment> CourseAssignments { get; set; }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");

            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID });
        }
    }
}
