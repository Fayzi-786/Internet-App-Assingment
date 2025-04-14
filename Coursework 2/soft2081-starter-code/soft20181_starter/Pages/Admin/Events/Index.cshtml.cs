using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;

namespace soft20181_starter.Pages.Admin.Events
{
    public class DeleteEventRequest
    {
        public int Id { get; set; }
    }
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
        public JsonResult OnPostDeleteEvent([FromBody] DeleteEventRequest request)
            {
            try
            {
                // delete event
                var eventToDelete = db.Events.Find(request.Id);
                if (eventToDelete != null)
                {
                    db.Events.Remove(eventToDelete);
                    db.SaveChanges();
                    return new JsonResult(new { success = true, message = "Event deleted succefully" });
                }
                return new JsonResult(new { success = false, message = "Could not find event" });

            }
            catch (Exception ex)
            {

                return new JsonResult(new { success = false, message = ex.Message });
            }
           
            
        }
        public IActionResult OnPostEdit(int id)
        {
            // redirect to edit page
            return RedirectToPage("Edit", new { id = id });
        }
    }   
}
