using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;

namespace soft20181_starter.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private EventAppDbContext db;
        public IndexModel(ILogger<IndexModel> logger, EventAppDbContext db)
        {
            this.db = db;
            _logger = logger;
        }
        [BindProperty]
        public List<Event> Events { get; set; } = new List<Event>();
        public void OnGet()
        {
            Events = db.Events.Take(3).ToList();
        }
    }
}
