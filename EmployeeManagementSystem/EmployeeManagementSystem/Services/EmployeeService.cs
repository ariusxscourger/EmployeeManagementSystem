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

    public async Task<List<Employee>> GetEmployeesAsync() => await _context.Employees
        .Include(e => e.Job)
        .Include(e => e.Department)
        .ToListAsync();

    public async Task<Employee> GetEmployeeByIdAsync(int id) =>
        await _context.Employees
            .Include(e => e.Job)
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

    public async Task AddEmployeeAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
