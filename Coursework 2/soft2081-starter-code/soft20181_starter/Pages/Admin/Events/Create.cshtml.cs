using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using soft20181_starter.Models;

namespace soft20181_starter.Pages.Admin.Events
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        public EventAppDbContext db { get; set; }
        private readonly IWebHostEnvironment _environment;

        public CreateModel(EventAppDbContext db, IWebHostEnvironment  environment)
        {
            this.db = db;
            _environment = environment;

        }
        [BindProperty]
        public Event Event { get; set; }
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        public PageResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Handle Image Upload
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                Event.Image = "/uploads/" + uniqueFileName; // Save path for displaying later
            }

            db.Events.Add(Event);
            await db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
