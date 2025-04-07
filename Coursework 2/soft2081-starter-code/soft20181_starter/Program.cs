using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using soft20181_starter.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' not found.");

// Add services to the container
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Events");
});

builder.Services.AddDbContext<EventAppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddIdentity<UsersInfo, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<EventAppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EventAppDbContext>();
        var userManager = services.GetRequiredService<UserManager<UsersInfo>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        context.Database.Migrate();
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.MapRazorPages();
app.Run();

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new EventAppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<EventAppDbContext>>()))
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<UsersInfo>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Seed roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed admin user
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new UsersInfo
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed events
            if (!context.Events.Any())
            {
                context.Events.AddRange(
                    new Event
                    {
                        Title = "Tech Conference 2025",
                        Date = DateTime.Parse("2025-03-15"),
                        Description = "Join us for the most innovative tech conference of the year!",
                        Location = "London, UK",
                        Image = "tech.jpg",
                        Time = "08:45 PM"
                    },
                    new Event
                    {
                        Title = "Java Intro",
                        Date = DateTime.Parse("2025-01-20"),
                        Description = "Introduction to OOP concept with Java.",
                        Location = "Nottingham Trent University, UK",
                        Image = "Java.jpg",
                        Time = "12:00 AM"
                    },
                    new Event
                    {
                        Title = "Unlocking Potential",
                        Date = DateTime.Parse("2025-06-10"),
                        Description = "Unlocking Potential: A Meeting on Advancing Education.",
                        Location = "Manchester, UK",
                        Image = "unlock-potential.jpg",
                        Time = "7:00 PM"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}