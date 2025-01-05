using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeeManagementSystem.Test
{
    public class IntegrationTests : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public IntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            _context = new ApplicationDbContext(options);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var manager = new Employee
            {
                LastName = "Manager",
                Email = "manager@example.com",
                PhoneNumber = "123456789",
                HireDate = DateTime.Now,
                JobId = 1,
                Salary = 90000,
                DepartmentId = 1,
                Status = "Active",
                Job = new Job { JobTitle = "Software Developer", MinSalary = 50000, MaxSalary = 120000 },
                Department = new Department {  DepartmentName = "Engineering", ManagerId = 2, Manager = null },
                Manager = null
            };

            _context.Employees.Add(manager);

            _context.Jobs.Add(new Job { JobTitle = "Software Developer", MinSalary = 50000, MaxSalary = 120000 });
            _context.Departments.Add(new Department {  DepartmentName = "Engineering", ManagerId = manager.EmployeeId, Manager = manager });
            _context.Employees.Add(new Employee
            {
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
            _context.Dispose(); // Clean up resources after each test
        }

        [Fact]
        public async Task AddEmployee_ShouldAddEmployee()
        {
            // Arrange
            var newEmployee = new Employee
            {
                LastName = "Smith",
                Email = "janesmith@example.com",
                PhoneNumber = "987654321",
                HireDate = DateTime.Now,
                JobId = 1,
                Salary = 70000,
                DepartmentId = 1,
                Status = "Active",
                Job = new Job {  JobTitle = "Software Developer", MinSalary = 50000, MaxSalary = 120000 },
                Department = new Department { DepartmentName = "Engineering", ManagerId = 2, Manager = null },
                Manager = null
            };

            // Act
            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();

            // Assert
            var employee = await _context.Employees.FindAsync(3);
            Assert.NotNull(employee);
            Assert.Equal("Smith", employee.LastName);
        }

        [Fact]
        public async Task GetEmployees_ShouldReturnAllEmployees()
        {
            // Act
            var employees = await _context.Employees
                .Include(e => e.Job)
                .Include(e => e.Department)
                .ToListAsync();

            // Assert
            Assert.Contains(employees, e => e.LastName == "Doe");
        }

        [Fact]
        public async Task UpdateEmployee_ShouldModifyEmployeeDetails()
        {
            // Arrange
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == 1);
            employee.Salary = 65000;

            // Act
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            // Assert
            var updatedEmployee = await _context.Employees.FindAsync(1);
            Assert.Equal(65000, updatedEmployee.Salary);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldRemoveEmployee()
        {
            // Arrange
            var employee = await _context.Employees.FindAsync(1);

            // Act
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            // Assert
            var deletedEmployee = await _context.Employees.FindAsync(1);
            Assert.Null(deletedEmployee);
        }

        [Fact]
        public async Task GetJobs_ShouldReturnAllJobs()
        {
            // Act
            var jobs = await _context.Jobs.ToListAsync();

            // Assert
            Assert.Contains(jobs, e => e.JobTitle == "Software Developer");
        }
    }
}


