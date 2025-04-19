using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using soft20181_starter.Models;

namespace soft20181_starter.Pages
{
    public class Event_DetailsModel : PageModel
    {
        public EventAppDbContext db{ get; set; }
        public Event EventDetails { get; set; }
        public bool IsRegistered { get; set; } = false;
        private readonly UserManager<UsersInfo> _userManager;

        public Event_DetailsModel(EventAppDbContext dbContext, UserManager<UsersInfo> userManager)
        {
            this.db = dbContext;
            _userManager = userManager;
        }
        public async Task<PageResult> OnGet(int Id)
        {
            var eventdetails = db.Events.Where(x => x.Id == Id).FirstOrDefault();
            if (eventdetails == null)
            {
                // TODO : Handle the case when the event is not found
                return Page();
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    var currentUser = await _userManager.GetUserAsync(User);
                    IsRegistered = await db.userEventRegistrations
                        .AnyAsync(r => r.EventId == Id && r.UserId == currentUser.Id);
                }
                EventDetails = eventdetails;
                return  Page();
            }
        }
        public async Task<IActionResult> OnPostRegisterAsync(int eventId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var existingRegistration = await db.userEventRegistrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == currentUser.Id);

            if (existingRegistration != null)
            {
                // User is already registered
                return BadRequest("You are already registered for this event.");
            }

            var registration = new UserEventRegistration
            {
                UserId = currentUser.Id,
                EventId = eventId
            };

            db.userEventRegistrations.Add(registration);
            await db.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostUnregisterAsync(int eventId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var registration = await db.userEventRegistrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == currentUser.Id);

            if (registration != null)
            {
                db.userEventRegistrations.Remove(registration);
                await db.SaveChangesAsync();
            }

            return new JsonResult(new { success = true });
        }
    }
}
