using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Disaster_Alleviation_Foundation_GOTGF_.Pages.App
{
    public class VolunteerLoginModel : PageModel
    {
        private readonly UserContext _context;
        public VolunteerLoginModel(UserContext context)
        {
            _context = context;
        }
        
        [BindProperty]
        public string LName { get; set; }

        [BindProperty]
        public string LEmail { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        // Login handler
        public async Task<IActionResult> OnPostAsync()
        {
            // Log in an existing volunteer
            if (ModelState.IsValid)
            {
                var volunteer = await _context.Volunteer
                    .FirstOrDefaultAsync(v => v.VName == LName && v.Email == LEmail);

                if (volunteer != null)
                {
                    // Successful login 
                    return RedirectToPage("/App/Member");
                }
                else
                {

                }
            }

            return Page();
        }
    }
}
