namespace EmployeeManagementSystem.Entity
{
    public class UserLogin
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
