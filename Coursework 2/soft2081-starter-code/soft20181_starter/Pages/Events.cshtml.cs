using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using soft20181_starter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace soft20181_starter.Pages
{
    [Authorize(Roles = "Admin")]
    public class EventsModel : PageModel
    {
        private readonly EventAppDbContext _dbContext;

        // Create a property to hold the list of events
        public List<Event> Events { get; set; }

        public EventsModel(EventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Modify OnGet to fetch events from the database
        public async Task OnGetAsync()
        {
            // Fetch the events from the database
            Events = await _dbContext.Events.ToListAsync();
        }
    }
}
