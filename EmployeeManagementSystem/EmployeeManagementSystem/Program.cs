using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Add this using directive
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. Add Blazor Server Services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// 4. Register Custom Services
builder.Services.AddScoped<EmployeeService>();

var app = builder.Build();

// Middleware Configuration
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Add this if you are using API controllers
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
