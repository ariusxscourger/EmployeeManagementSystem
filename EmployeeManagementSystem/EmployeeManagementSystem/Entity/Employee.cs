namespace EmployeeManagementSystem.Entity
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public int JobId { get; set; }
        public decimal Salary { get; set; }
        public int? ManagerId { get; set; }
        public int DepartmentId { get; set; }
        public string Status { get; set; } = "Active";

        public Job Job { get; set; }
        public Department Department { get; set; }
        public Employee Manager { get; set; }
    }

}
