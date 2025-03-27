using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using soft20181_starter.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found."); ;

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizePage("/Events"); // Enforce authentication for the Events page
});

var Configuration = builder.Configuration;
builder.Services.AddDbContext<EventAppDbContext>(options =>
   options.UseSqlite(Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<UsersInfo, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddDefaultUI()
                .AddEntityFrameworkStores<EventAppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapRazorPages();

app.Run();
