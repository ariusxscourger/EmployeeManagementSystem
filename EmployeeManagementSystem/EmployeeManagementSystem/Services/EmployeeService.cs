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

    // READ
    public async Task<List<Employee>> GetEmployeesAsync()
    {
        return await _context.Employees
            .Include(e => e.Job)
            .Include(e => e.Department)
            .ToListAsync();
    }
    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Job)
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);
    }
    public async Task<List<Job>> GetJobsAsync()
    {
        return await _context.Jobs.ToListAsync();
    }
    public async Task<Job?> GetJobByIdAsync(int id)
    {
        return await _context.Jobs
            .FirstOrDefaultAsync(j => j.JobId == id);
    }
    public async Task<List<Employee>> GetManagersAsync()
    {
        return await _context.Employees
            .Where(e => e.Job != null && e.Job.JobTitle == "Manager")
            .ToListAsync();
    }
    public async Task<List<Department>> GetDepartmentsAsync()
    {
        return await _context.Departments.ToListAsync();
    }
    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentId == id);
    }

    // CREATE
    public async Task AddEmployeeAsync(Employee employee)
    {
        // Validate foreign keys
        if (!await _context.Departments.AnyAsync(d => d.DepartmentId == employee.DepartmentId))
        {
            throw new Exception($"DepartmentId {employee.DepartmentId} does not exist.");
        }

        if (!await _context.Jobs.AnyAsync(j => j.JobId == employee.JobId))
        {
            throw new Exception($"JobId {employee.JobId} does not exist.");
        }

        if (employee.ManagerId.HasValue &&
            !await _context.Employees.AnyAsync(m => m.EmployeeId == employee.ManagerId))
        {
            throw new Exception($"ManagerId {employee.ManagerId} does not exist.");
        }

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
        // Validate ManagerId
        if (department.ManagerId.HasValue &&
            !await _context.Employees.AnyAsync(m => m.EmployeeId == department.ManagerId))
        {
            throw new Exception($"ManagerId {department.ManagerId} does not exist.");
        }

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
    }

    // UPDATE
    public async Task UpdateEmployeeAsync(Employee employee)
    {
        // Validate foreign keys
        if (!await _context.Departments.AnyAsync(d => d.DepartmentId == employee.DepartmentId))
        {
            throw new Exception($"DepartmentId {employee.DepartmentId} does not exist.");
        }

        if (!await _context.Jobs.AnyAsync(j => j.JobId == employee.JobId))
        {
            throw new Exception($"JobId {employee.JobId} does not exist.");
        }

        if (employee.ManagerId.HasValue &&
            !await _context.Employees.AnyAsync(m => m.EmployeeId == employee.ManagerId))
        {
            throw new Exception($"ManagerId {employee.ManagerId} does not exist.");
        }

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
        // Validate ManagerId
        if (department.ManagerId.HasValue &&
            !await _context.Employees.AnyAsync(m => m.EmployeeId == department.ManagerId))
        {
            throw new Exception($"ManagerId {department.ManagerId} does not exist.");
        }

        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }

    // DELETE
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

    public async Task<List<EmployeeAudit>> GetAuditsAsync()
    {
        return await _context.EmployeeAudits.ToListAsync();
    }
}
