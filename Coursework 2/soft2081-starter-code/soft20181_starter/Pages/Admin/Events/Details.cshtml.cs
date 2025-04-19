using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using soft20181_starter.Models;

namespace soft20181_starter.Pages.Admin.Events
{
    public class DetailsModel : PageModel
    {
        private readonly EventAppDbContext _context;

        public DetailsModel(EventAppDbContext context)
        {
            _context = context;
        }

        public Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (Event == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
