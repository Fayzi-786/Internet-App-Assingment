using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using soft20181_starter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soft20181_starter.Pages
{
    public class EventsModel : PageModel
    {
        private readonly EventAppDbContext _dbContext;

        public List<Event> Events { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CategoryFilter { get; set; }

        public EventsModel(EventAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnGetAsync()
        {
            IQueryable<Event> query = _dbContext.Events;

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(e =>
                    e.Title.Contains(SearchTerm) ||
                    e.Location.Contains(SearchTerm) ||
                    e.Description.Contains(SearchTerm));
            }

            if (!string.IsNullOrEmpty(CategoryFilter) && CategoryFilter != "all")
            {
                query = query.Where(e => e.Category != null &&
                    e.Category.ToLower() == CategoryFilter.ToLower());
            }

            Events = await query.OrderBy(e => e.Date).ToListAsync();
        }
    }

}