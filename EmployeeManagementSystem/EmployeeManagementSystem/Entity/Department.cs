namespace EmployeeManagementSystem.Entity
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? ManagerId { get; set; }

        public Employee Manager { get; set; }
    }

}