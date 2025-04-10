using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using soft20181_starter.Models;

namespace soft20181_starter.Pages
{
    public class Event_DetailsModel : PageModel
    {
        public EventAppDbContext db{ get; set; }
        public Event EventDetails { get; set; }
        public Event_DetailsModel(EventAppDbContext dbContext)
        {
            this.db = dbContext;    
        }
        public PageResult OnGet(int Id)
        {
            var eventdetails = db.Events.Where(x => x.Id == Id).FirstOrDefault();
            if (eventdetails == null)
            {
                // TODO : Handle the case when the event is not found
                return Page();
            }
            else
            {
                EventDetails = eventdetails;
                return Page();
            }
        }
    }
}
