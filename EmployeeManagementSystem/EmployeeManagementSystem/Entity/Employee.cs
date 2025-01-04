namespace EmployeeManagementSystem.Entity
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public required string LastName { get; set; } //= null;
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public int JobId { get; set; }
        public decimal Salary { get; set; }
        public int? ManagerId { get; set; }
        public int DepartmentId { get; set; }
        public string Status { get; set; } = "Active";

        public required Job Job { get; set; }
        public required Department Department { get; set; }
        public required Employee Manager { get; set; }
    }

}
