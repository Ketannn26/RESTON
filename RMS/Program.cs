using Microsoft.EntityFrameworkCore;
using RMS.Data;
using RMS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<RMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;                // Enhance security
    options.Cookie.IsEssential = true;            // Ensure the session cookie is essential
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

var app = builder.Build();

// Seed the Admin Account
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RMSDbContext>();
    SeedAdminUser(dbContext);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseAuthorization();

// Define endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();

// Seed admin account if it doesn't exist
void SeedAdminUser(RMSDbContext dbContext)
{
    var adminEmail = "admin@reston.com";
    var adminUser = dbContext.Users.FirstOrDefault(u => u.Email == adminEmail);

    if (adminUser == null)
    {
        dbContext.Users.Add(new User
        {
            FullName = "Administrator",
            Email = adminEmail,
            Password = "Admin@123", // Consider hashing the password here
            Role = "Admin",
            PhoneNumber = "0000000000" // Placeholder phone number
        });
        dbContext.SaveChanges();
        Console.WriteLine("Admin account seeded successfully.");
    }
    else
    {
        Console.WriteLine("Admin account already exists.");
    }
}
