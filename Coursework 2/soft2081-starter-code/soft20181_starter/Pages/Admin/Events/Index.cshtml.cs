using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;

namespace soft20181_starter.Pages.Admin.Events
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public EventAppDbContext db { get; set; }
        public List<Event> Events { get; set; }
        public IndexModel(EventAppDbContext db)
        {
            this.db = db;
        }
        public PageResult OnGet()
        {
            // load all events
            Events = db.Events.ToList();
            return Page();
        }
        public PageResult OnPostDelete(int id)
        {
            // delete event
            var eventToDelete = db.Events.Find(id);
            if (eventToDelete != null)
            {
                db.Events.Remove(eventToDelete);
                db.SaveChanges();
            }
            return Page();
        }
        public IActionResult OnPostEdit(int id)
        {
            // redirect to edit page
            return RedirectToPage("Edit", new { id = id });
        }
    }
}
