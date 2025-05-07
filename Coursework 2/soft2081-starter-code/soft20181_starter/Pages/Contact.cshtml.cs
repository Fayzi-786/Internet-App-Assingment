using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;

namespace soft20181_starter.Pages
{
    public class ContactModel : PageModel
    {

        // injecting Db Context into razor page
        public EventAppDbContext _db { get; set; }
        public ContactModel (EventAppDbContext db)
        {
            _db = db;
        }


        [BindProperty]
        public Contact ContactInfo { get; set; }
        public String FormMessage { get; set; }

        public void OnGet()
        {
            ContactInfo = new Contact();

        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                FormMessage = "Your Message received Succusfully. We will be with you shortly";

                // Save to Database
                _db.ContactTable.Add(ContactInfo);
                _db.SaveChanges();
                // Show success message
                // return
                return RedirectToPage("Contact");
            }
            else
            {
                FormMessage = "Error while sending your message!";
                return Page();            
            }

            


        }
    }
}
