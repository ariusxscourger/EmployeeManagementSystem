using Blazored.LocalStorage;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
public class UserLoginService
{
    private readonly ApplicationDbContext _context;
    private readonly ILocalStorageService _localStorage;

    public UserLoginService(ApplicationDbContext context, ILocalStorageService localStorage)
    {
        _context = context;
        _localStorage = localStorage;
    }

    // Check if the user is logged in
    public async Task<bool> IsLoggedInAsync()
    {
        var userId = await GetLoggedInUserIdAsync();
        return userId != null && await _context.UserLogins.AnyAsync(ul => ul.UserId == userId && ul.IsActive);
    }

    private async Task<int?> GetLoggedInUserIdAsync()
    {
        return await _localStorage.GetItemAsync<int?>("userId");
    }
    public async Task LoginAsync(int userId)
    {
        await _localStorage.SetItemAsync("userId", userId);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("userId");
    }
    // Retrieve a UserLogin by UserId, including associated Employee
    public async Task<UserLogin?> GetLoginAsync(int userId)
    {
        return await _context.UserLogins
            .Include(ul => ul.UserId) // Correct navigation property
            .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.IsActive);
    }

    // Validate login by email and password
    public async Task<bool> ValidateLoginAsync(string email, string password)
    {
        var userLogin = await _context.UserLogins.FirstOrDefaultAsync(ul => ul.Email == email && ul.IsActive);
        return userLogin != null && (password == userLogin.PasswordHash);
    }

    // Add a new UserLogin
    public async Task AddLoginAsync(UserLogin login)
    {
        var userLogin = new UserLogin
        {
            UserId = login.UserId,
            FirstName = login.FirstName,
            LastName = login.LastName,
            Email = login.Email,
            PasswordHash = login.PasswordHash,
            IsActive = login.IsActive,
        };

        _context.UserLogins.Add(userLogin);
        await _context.SaveChangesAsync();
    }

    // Check if an email already exists
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.UserLogins.AnyAsync(ul => ul.Email == email);
    }
    public string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
