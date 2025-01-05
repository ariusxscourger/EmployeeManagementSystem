using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;

public class EmployeeService
{
    private readonly ApplicationDbContext _context;

    public EmployeeService(ApplicationDbContext context)
    {
        _context = context;
    }

    //READ
    public async Task<List<Employee>> GetEmployeesAsync() => await _context.Employees
        .Include(e => e.Job)
        .Include(e => e.Department)
        .ToListAsync();

    public async Task<Employee> GetEmployeeByIdAsync(int id) =>
        await _context.Employees
            .Include(e => e.Job)
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);
    public async Task<List<Job>> GetJobsAsync() => await _context.Jobs.ToListAsync();
    public async Task<List<Department>> GetDepartmentsAsync() => await _context.Departments.ToListAsync();

    //CREATE
    public async Task AddEmployeeAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }
    public async Task AddJobAsync(Job job)
    {
        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
    }
    public async Task AddDepartmentAsync(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
    }

    //UPDATE
    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateJobAsync(Job job)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateDepartmentAsync(Department department)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }
    //DELETE
    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
    public async Task DeleteJobAsync(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job != null)
        {
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
        }
    }
    public async Task DeleteDepartmentAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddAuditAsync(EmployeeAudit newAudit)
    {
        _context.EmployeeAudits.Add(newAudit);
        await _context.SaveChangesAsync();
    }

}
