using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace soft20181_starter.Pages.Admin.Events
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly EventAppDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public EditModel(EventAppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [BindProperty]
        public Event Event { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; } // Made nullable

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Event = await _db.Events.FindAsync(id);
            if (Event == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var eventToUpdate = await _db.Events.FindAsync(id);
            if (eventToUpdate == null)
            {
                return NotFound();
            }

            // Handle image upload only if a new image is provided
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(eventToUpdate.Image))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, eventToUpdate.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save new image
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                eventToUpdate.Image = "/uploads/" + uniqueFileName;
            }

            // Update other fields
            eventToUpdate.Title = Event.Title;
            eventToUpdate.Location = Event.Location;
            eventToUpdate.Description = Event.Description;
            eventToUpdate.Date = Event.Date;
            eventToUpdate.Time = Event.Time;

            try
            {
                await _db.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(Event.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        private bool EventExists(int id)
        {
            return _db.Events.Any(e => e.Id == id);
        }
    }
}