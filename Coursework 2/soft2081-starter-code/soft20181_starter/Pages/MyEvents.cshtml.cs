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
using NReco.PdfGenerator;

namespace soft20181_starter.Pages
{
    [Authorize]
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

        [BindProperty]
        public int EID { get; set; }

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

        public IActionResult OnPostDownloadTicket()
        {
            var eventDetails = _context.Events.FirstOrDefault(e => e.Id == EID);
            if (eventDetails == null)
            {
                return NotFound("Event not found");
            }

            var html = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .ticket {{ 
                        border: 2px solid #000; 
                        padding: 20px; 
                        width: 300px;
                        margin: 0 auto;
                    }}
                    .header {{ 
                        text-align: center; 
                        margin-bottom: 20px;
                    }}
                    .event-title {{
                        font-size: 18px;
                        font-weight: bold;
                        margin-bottom: 15px;
                    }}
                    .detail {{ margin-bottom: 10px; }}
                    .qr-placeholder {{
                        width: 100px;
                        height: 100px;
                        background-color: #f0f0f0;
                        margin: 15px auto;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                    }}
                </style>
            </head>
            <body>
                <div class='ticket'>
                    <div class='header'>
                        <h1>EVENT TICKET</h1>
                        <p>Admit One</p>
                    </div>
                    
                    <div class='event-title'>{eventDetails.Title}</div>
                    
                    <div class='detail'><strong>Attendee:</strong> {User.Identity?.Name}</div>
                    <div class='detail'><strong>Date:</strong> {eventDetails.Date.ToString("MMMM dd, yyyy")}</div>
                    <div class='detail'><strong>Time:</strong> {eventDetails.Time}</div>
                    <div class='detail'><strong>Location:</strong> {eventDetails.Location}</div>
                    
                    <div class='qr-placeholder'>QR Code</div>
                    
                    <div style='text-align: center; margin-top: 20px; font-size: 12px;'>
                        Present this ticket at the event entrance
                    </div>
                </div>
            </body>
            </html>";

            var pdf = new HtmlToPdfConverter().GeneratePdf(html);
            return File(pdf, "application/pdf", $"Ticket-{eventDetails.Title}-{eventDetails.Id}.pdf");
        }

        public class RegisteredEventDto
        {
            public Event Event { get; set; } = default!;
            public DateTime RegistrationDate { get; set; }
        }
    }
}