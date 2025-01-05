using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Entity
{
    public class Job
    {
        public int JobId { get; set; }
        public required string JobTitle { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
    }

}