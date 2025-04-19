using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using soft20181_starter.Models;

public class MyEventsModel : PageModel
{
    private readonly EventAppDbContext _context;
    private readonly UserManager<UsersInfo> _userManager;

    public MyEventsModel(EventAppDbContext context, UserManager<UsersInfo> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public List<RegisteredEventDto> RegisteredEvents { get; set; } = new List<RegisteredEventDto>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Identity/Account/Login", new { area = "Identity" });
        }

        var currentUser = await _userManager.GetUserAsync(User);

        RegisteredEvents = await _context.userEventRegistrations
            .Where(r => r.UserId == currentUser.Id)
            .Include(r => r.Event)
            .OrderByDescending(r => r.Event.Date)
            .Select(r => new RegisteredEventDto
            {
                Event = r.Event,
                RegistrationDate = r.RegistrationDate
            })
            .ToListAsync();

        return Page();
    }
}

public class RegisteredEventDto
{
    public Event Event { get; set; }
    public DateTime RegistrationDate { get; set; }
}