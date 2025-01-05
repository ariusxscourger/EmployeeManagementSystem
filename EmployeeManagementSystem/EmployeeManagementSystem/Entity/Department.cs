namespace EmployeeManagementSystem.Entity
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
        public int? ManagerId { get; set; }
        public required Employee? Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>(); // Reverse navigation


    }

}