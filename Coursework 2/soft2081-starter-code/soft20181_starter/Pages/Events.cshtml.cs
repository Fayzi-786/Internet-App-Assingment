using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;

namespace soft20181_starter.Pages
{
    [Authorize(Roles ="Admin")]
    public class EventsModel : PageModel
    {
        private readonly EventAppDbContext dbContext;
        public void OnGet()
        {
        }
    }
}
