using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Entity
{
    public class Employee
    {
            public int EmployeeId { get; set; }

            // Required Basic Details
            public required string FirstName { get; set; }
            public required string LastName { get; set; }
            public required string Email { get; set; }
            public required string PhoneNumber { get; set; }
            public DateTime HireDate { get; set; }
            public decimal Salary { get; set; }

            // Foreign Key Relationships
            public int JobId { get; set; }
            public int DepartmentId { get; set; }
            public int? ManagerId { get; set; } // Nullable for employees without a manager

            // Status
            public string Status { get; set; } = "Active";

            // Navigation Properties
            public Job? Job { get; set; } // Optional, loaded when needed
            public Department? Department { get; set; } // Optional, loaded when needed
            public Employee? Manager { get; set; } // Optional, prevents circular reference issues
    }
}
