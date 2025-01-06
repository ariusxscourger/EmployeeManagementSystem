using Blazored.LocalStorage;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeManagementSystem.Test
{
    public class UserLoginServiceTests
    {
        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<ILocalStorageService> _mockLocalStorage;
        private readonly UserLoginService _userLoginService;

        public UserLoginServiceTests()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _mockLocalStorage = new Mock<ILocalStorageService>();
            _userLoginService = new UserLoginService(_mockContext.Object, _mockLocalStorage.Object);
        }

        [Fact]
        public async Task IsLoggedInAsync_ShouldReturnTrue_WhenUserIsLoggedIn()
        {
            // Arrange
            _mockLocalStorage.Setup(ls => ls.GetItemAsync<int?>("userId", It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mockContext.Setup(ctx => ctx.UserLogins.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserLogin, bool>>>(), default)).ReturnsAsync(true);

            // Act
            var result = await _userLoginService.IsLoggedInAsync();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsLoggedInAsync_ShouldReturnFalse_WhenUserIsNotLoggedIn()
        {
            // Arrange
            _mockLocalStorage.Setup(ls => ls.GetItemAsync<int?>("userId", It.IsAny<CancellationToken>())).ReturnsAsync((int?)null);

            // Act
            var result = await _userLoginService.IsLoggedInAsync();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task LoginAsync_ShouldSetUserIdInLocalStorage()
        {
            // Arrange
            var userId = 1;

            // Act
            await _userLoginService.LoginAsync(userId);

            // Assert
            _mockLocalStorage.Verify(ls => ls.SetItemAsync("userId", userId, default), Times.Once);
        }

        [Fact]
        public async Task LogoutAsync_ShouldRemoveUserIdFromLocalStorage()
        {
            // Act
            await _userLoginService.LogoutAsync();

            // Assert
            _mockLocalStorage.Verify(ls => ls.RemoveItemAsync("userId", default), Times.Once);
        }

        [Fact]
        public async Task GetLoginAsync_ShouldReturnUserLogin_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var userLogin = new UserLogin { UserId = userId, Email = "test@example.com", PasswordHash = "hashedpassword", IsActive = true };
            _mockContext.Setup(ctx => ctx.UserLogins.Include(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserLogin, object>>>()).FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserLogin, bool>>>(), default)).ReturnsAsync(userLogin);

            // Act
            var result = await _userLoginService.GetLoginAsync(userId);

            // Assert
            Assert.Equal(userLogin, result);
        }

        [Fact]
        public async Task GetLoginAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _mockContext.Setup(ctx => ctx.UserLogins.Include(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserLogin, object>>>()).FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserLogin, bool>>>(), default)).ReturnsAsync((UserLogin?)null);

            // Act
            var result = await _userLoginService.GetLoginAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ValidateLoginAsync_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";
            var passwordHash = _userLoginService.HashPassword(password);
            var userLogin = new UserLogin { Email = email, PasswordHash = passwordHash, IsActive = true };
            _mockContext.Setup(ctx => ctx.UserLogins.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserLogin, bool>>>(), default)).ReturnsAsync(userLogin);

            // Act
            var result = await _userLoginService.ValidateLoginAsync(email, password);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ValidateLoginAsync_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";
            _mockContext.Setup(ctx => ctx.UserLogins.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserLogin, bool>>>(), default)).ReturnsAsync((UserLogin?)null);

            // Act
            var result = await _userLoginService.ValidateLoginAsync(email, password);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddLoginAsync_ShouldAddUserLogin()
        {
            // Arrange
            var userLogin = new UserLogin
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PasswordHash = "hashedpassword",
                IsActive = true
            };

            // Act
            await _userLoginService.AddLoginAsync(userLogin);

            // Assert
            _mockContext.Verify(ctx => ctx.UserLogins.Add(It.Is<UserLogin>(ul => ul == userLogin)), Times.Once);
            _mockContext.Verify(ctx => ctx.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            var email = "test@example.com";
            _mockContext.Setup(ctx => ctx.UserLogins.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserLogin, bool>>>(), default)).ReturnsAsync(true);

            // Act
            var result = await _userLoginService.EmailExistsAsync(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task EmailExistsAsync_ShouldReturnFalse_WhenEmailDoesNotExist()
        {
            // Arrange
            var email = "test@example.com";
            _mockContext.Setup(ctx => ctx.UserLogins.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<UserLogin, bool>>>(), default)).ReturnsAsync(false);

            // Act
            var result = await _userLoginService.EmailExistsAsync(email);

            // Assert
            Assert.False(result);
        }
    }
}
