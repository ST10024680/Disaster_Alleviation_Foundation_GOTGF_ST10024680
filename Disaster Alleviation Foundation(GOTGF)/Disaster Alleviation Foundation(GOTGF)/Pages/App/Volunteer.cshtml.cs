using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Disaster_Alleviation_Foundation_GOTGF_.Pages.App
{
    public class VolunteerModel : PageModel
    {
        private readonly UserContext _context;

        public VolunteerModel(UserContext context)
        {
            _context = context;
        }



        [BindProperty]
        public string VName { get; set; } = string.Empty;

        [BindProperty]
        public string VEmail { get; set; } = string.Empty;

        [BindProperty]
        public string VPhoneNum { get; set; } = string.Empty;



        public IActionResult OnGet()
        {
            return Page();
        }

        // Register handler
        public async Task<IActionResult> OnPostAsync()
        {
            // Register a new volunteer
            if (ModelState.IsValid)
            {
                var newVolunteer = new Volunteer
                {
                    VName = VName,
                    Email = VEmail,
                    VPhoneNumber = VPhoneNum
                };

                _context.Volunteer.Add(newVolunteer);
                await _context.SaveChangesAsync();
                return RedirectToPage("/App/Member");
            }

            return Page();
        }


    }

}