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


            // Generate PDF using NReco
            var html = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 20px;
                }}
                .ticket {{
                    background: #fff;
                    border-radius: 12px;
                    box-shadow: 0 4px 12px rgba(0,0,0,0.1);
                    padding: 30px;
                    max-width: 400px;
                    margin: auto;
                    border-left: 5px solid #4A90E2;
                }}
                .header {{
                    text-align: center;
                    border-bottom: 1px solid #ddd;
                    padding-bottom: 15px;
                    margin-bottom: 20px;
                }}
                .header h1 {{
                    font-size: 24px;
                    color: #4A90E2;
                    margin: 0;
                }}
                .header p {{
                    font-size: 14px;
                    color: #888;
                    margin: 4px 0 0;
                }}
                .event-title {{
                    font-size: 20px;
                    font-weight: 600;
                    margin-bottom: 15px;
                    color: #333;
                }}
                .detail {{
                    margin-bottom: 10px;
                    font-size: 14px;
                    color: #555;
                }}
                .detail strong {{
                    color: #000;
                }}
                .qr-placeholder {{
                    width: 120px;
                    height: 120px;
                    background-color: #e0e0e0;
                    margin: 25px auto 15px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    font-size: 12px;
                    color: #777;
                    border-radius: 6px;
                }}
                .footer-note {{
                    text-align: center;
                    font-size: 12px;
                    color: #888;
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
        
                <div class='footer-note'>
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