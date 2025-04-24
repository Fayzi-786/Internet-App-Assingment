using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using soft20181_starter.Models;

namespace soft20181_starter.Pages
{
    [Authorize]                              // only authenticated users can hit this page
    public class MyEventsModel : PageModel
    {
        private readonly EventAppDbContext _context;
        private readonly UserManager<UsersInfo> _userManager;

        public MyEventsModel(EventAppDbContext context,
                             UserManager<UsersInfo> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<RegisteredEventDto> RegisteredEvents { get; private set; } = new();

        /* ─────────────── GET: /MyEvents ─────────────── */
        public async Task<IActionResult> OnGetAsync()
        {
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

        /* ─────── POST (AJAX): /MyEvents?handler=Unregister ─────── */
        public async Task<IActionResult> OnPostUnregisterAsync(int eventId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var registration = await _context.userEventRegistrations
                .FirstOrDefaultAsync(r => r.UserId == currentUser.Id && r.EventId == eventId);

            if (registration is null)
                return new JsonResult(new { success = false });

            _context.userEventRegistrations.Remove(registration);
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        /* small DTO used by the page */
        public class RegisteredEventDto
        {
            public Event Event { get; set; } = default!;
            public DateTime RegistrationDate { get; set; }
        }
    }
}
