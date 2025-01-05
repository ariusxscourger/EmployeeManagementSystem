namespace EmployeeManagementSystem.Entity
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
        public int? ManagerId { get; set; }
        public ICollection<Employee> Employees { get; set; } // For reverse navigation
        public required Employee Manager { get; set; }
    }

}