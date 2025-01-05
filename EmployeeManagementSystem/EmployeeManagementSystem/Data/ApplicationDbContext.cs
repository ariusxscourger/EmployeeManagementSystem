using EmployeeManagementSystem.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EmployeeManagementSystem.Data
{ 
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<EmployeeAudit> EmployeeAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Department>()
               .HasOne(d => d.Manager) // Assuming this navigation property
               .WithMany()
               .HasForeignKey(d => d.ManagerId)
               .OnDelete(DeleteBehavior.NoAction); // Prevent cascade delete

            builder.Entity<Employee>()
            .Property(e => e.Salary)
            .HasColumnType("decimal(18,2)");

            builder.Entity<Job>()
            .Property(e => e.MaxSalary)
            .HasColumnType("decimal(18,2)");

            builder.Entity<Job>()
            .Property(e => e.MinSalary)
            .HasColumnType("decimal(18,2)");

            builder.Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany()
                .HasForeignKey(e => e.ManagerId);

            builder.Entity<Employee>()
                .HasOne(e => e.Job)
                .WithMany()
                .HasForeignKey(e => e.JobId);

            builder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId);

            builder.Entity<EmployeeAudit>()
                .HasKey(a => a.AuditId); 
        }
    }

}
