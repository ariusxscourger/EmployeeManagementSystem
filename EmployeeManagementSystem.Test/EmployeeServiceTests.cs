using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagementSystem.Test
{
    public class EmployeeServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _employeeService = new EmployeeService(_context);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var manager = new Employee
            {
                FirstName = "John",
                LastName = "Manager",
                Email = "manager@example.com",
                PhoneNumber = "123456789",
                HireDate = DateTime.Now,
                JobId = 1,
                Salary = 90000,
                DepartmentId = 1,
                Status = "Active",
                Job = new Job { JobTitle = "Software Developer", MinSalary = 50000, MaxSalary = 120000 },
                Department = new Department { DepartmentName = "Engineering", ManagerId = 2, Manager = null },
                Manager = null
            };

            _context.Employees.Add(manager);

            _context.Jobs.Add(new Job { JobTitle = "Software Developer", MinSalary = 50000, MaxSalary = 120000 });
            _context.Departments.Add(new Department { DepartmentName = "Engineering", ManagerId = manager.EmployeeId, Manager = manager });
            _context.Employees.Add(new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                PhoneNumber = "123456789",
                HireDate = DateTime.Now,
                JobId = 1,
                Salary = 60000,
                DepartmentId = 1,
                Status = "Active",
                Job = new Job { JobTitle = "Software Developer", MinSalary = 50000, MaxSalary = 120000 },
                Department = new Department { DepartmentName = "Engineering", ManagerId = 2, Manager = manager },
                Manager = manager
            });

            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetEmployeesAsync_ShouldReturnAllEmployees()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            Assert.Contains(employees, e => e.LastName == "Doe");
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ShouldReturnEmployee()
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(1);
            Assert.NotNull(employee);
            Assert.Equal("Manager", employee.LastName);
        }

        [Fact]
        public async Task GetJobsAsync_ShouldReturnAllJobs()
        {
            var jobs = await _employeeService.GetJobsAsync();
            Assert.Contains(jobs, j => j.JobTitle == "Software Developer");
        }

        [Fact]
        public async Task GetJobByIdAsync_ShouldReturnJob()
        {
            var job = await _employeeService.GetJobByIdAsync(1);
            Assert.NotNull(job);
            Assert.Equal("Software Developer", job.JobTitle);
        }

        [Fact]
        public async Task GetManagersAsync_ShouldReturnAllManagers()
        {
            var managers = await _employeeService.GetManagersAsync();
            Assert.Contains(managers, m => m.LastName == "Manager");
        }

        [Fact]
        public async Task GetDepartmentsAsync_ShouldReturnAllDepartments()
        {
            var departments = await _employeeService.GetDepartmentsAsync();
            Assert.Contains(departments, d => d.DepartmentName == "Engineering");
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_ShouldReturnDepartment()
        {
            var department = await _employeeService.GetDepartmentByIdAsync(1);
            Assert.NotNull(department);
            Assert.Equal("Engineering", department.DepartmentName);
        }

        [Fact]
        public async Task AddEmployeeAsync_ShouldAddEmployee()
        {
            var newEmployee = new Employee
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "janesmith@example.com",
                PhoneNumber = "987654321",
                HireDate = DateTime.Now,
                JobId = 1,
                Salary = 70000,
                DepartmentId = 1,
                Status = "Active"
            };

            await _employeeService.AddEmployeeAsync(newEmployee);

            var employee = await _context.Employees.FindAsync(newEmployee.EmployeeId);
            Assert.NotNull(employee);
            Assert.Equal("Smith", employee.LastName);
        }

        [Fact]
        public async Task AddJobAsync_ShouldAddJob()
        {
            var newJob = new Job
            {
                JobTitle = "Tester",
                MinSalary = 40000,
                MaxSalary = 80000
            };

            await _employeeService.AddJobAsync(newJob);

            var job = await _context.Jobs.FindAsync(newJob.JobId);
            Assert.NotNull(job);
            Assert.Equal("Tester", job.JobTitle);
        }

        [Fact]
        public async Task AddDepartmentAsync_ShouldAddDepartment()
        {
            var newDepartment = new Department
            {
                DepartmentName = "HR",
                ManagerId = 1,
                Manager = new Employee
                {
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Manager",
                    Email = "manager@example.com",
                    PhoneNumber = "123456789",
                    HireDate = DateTime.Now,
                    JobId = 1,
                    Salary = 90000,
                    DepartmentId = 1,
                    Status = "Active"
                }
            };

            await _employeeService.AddDepartmentAsync(newDepartment);

            var department = await _context.Departments.FindAsync(newDepartment.DepartmentId);
            Assert.NotNull(department);
            Assert.Equal("HR", department.DepartmentName);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldUpdateEmployee()
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == 1);
            employee.Salary = 95000;

            await _employeeService.UpdateEmployeeAsync(employee);

            var updatedEmployee = await _context.Employees.FindAsync(1);
            Assert.Equal(95000, updatedEmployee.Salary);
        }

        [Fact]
        public async Task UpdateJobAsync_ShouldUpdateJob()
        {
            var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == 1);
            job.MaxSalary = 130000;

            await _employeeService.UpdateJobAsync(job);

            var updatedJob = await _context.Jobs.FindAsync(1);
            Assert.Equal(130000, updatedJob.MaxSalary);
        }

        [Fact]
        public async Task UpdateDepartmentAsync_ShouldUpdateDepartment()
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == 1);
            department.DepartmentName = "R&D";

            await _employeeService.UpdateDepartmentAsync(department);

            var updatedDepartment = await _context.Departments.FindAsync(1);
            Assert.Equal("R&D", updatedDepartment.DepartmentName);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ShouldDeleteEmployee()
        {
            await _employeeService.DeleteEmployeeAsync(1);

            var deletedEmployee = await _context.Employees.FindAsync(1);
            Assert.Null(deletedEmployee);
        }

        [Fact]
        public async Task DeleteJobAsync_ShouldDeleteJob()
        {
            await _employeeService.DeleteJobAsync(1);

            var deletedJob = await _context.Jobs.FindAsync(1);
            Assert.Null(deletedJob);
        }

        [Fact]
        public async Task DeleteDepartmentAsync_ShouldDeleteDepartment()
        {
            await _employeeService.DeleteDepartmentAsync(1);

            var deletedDepartment = await _context.Departments.FindAsync(1);
            Assert.Null(deletedDepartment);
        }

        [Fact]
        public async Task AddAuditAsync_ShouldAddAudit()
        {
            var newAudit = new EmployeeAudit
            {
                EmployeeId = 1,
                ActionType = "Update",
                ChangedData = "Salary updated to 95000",
                ChangedBy = "Admin",
                ChangedAt = DateTime.Now
            };

            await _employeeService.AddAuditAsync(newAudit);

            var audit = await _context.EmployeeAudits.FindAsync(newAudit.AuditId);
            Assert.NotNull(audit);
            Assert.Equal("Update", audit.ActionType);
        }

        [Fact]
        public async Task GetAuditsAsync_ShouldReturnAllAudits()
        {
            var audits = await _employeeService.GetAuditsAsync();
            Assert.NotEmpty(audits);
        }
    }
}