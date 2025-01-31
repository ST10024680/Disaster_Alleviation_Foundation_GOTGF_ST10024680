using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disaster_Alleviation_Foundation_GOTGF_.Pages.App
{
    public class DonateModel : PageModel
    {
        private readonly UserContext _context;

        public IList<ResourceDonation> ResourceDonation { get; set; } = new List<ResourceDonation>();

        public DonateModel(UserContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // This property will hold the new data
        [BindProperty]
        public ResourceDonation NewResourceDonation { get; set; } = new ResourceDonation();



        // Fetches the Resource Donations data when the page is loaded
        public async Task OnGetAsync()
        {
            try
            {
                // Fetch Resource Donation from the database
                ResourceDonation = await _context.ResourceDonation.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error (consider using a logging framework)
                ModelState.AddModelError(string.Empty, "Unable to load Resource Donation. Please try again later.");
                Console.WriteLine($"Error loading Resource Donation: {ex.Message}"); // Logging for debugging
            }

        }

        // Handles monetary donation submission
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Return the page with validation errors if any
                return Page();
            }
            try
            {

                // Set the donation date to current date
                NewResourceDonation.DonationDate = DateTime.Now;

                // Save the donation to the database
                _context.ResourceDonation.Add(NewResourceDonation);
                await _context.SaveChangesAsync();

                // Show a success message
                TempData["SuccessMessage"] = "donation submitted successfully!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // Log the error 
                ModelState.AddModelError(string.Empty, "Unable to save the donation. Please try again later.");
                Console.WriteLine($"Error saving donation: {ex.Message}");
                return Page();
            }



        }
        // Marks a resource donation as distributed
        public async Task<IActionResult> OnPostMarkAsDistributedAsync(int donationId)
        {
            var donation = await _context.ResourceDonation.FindAsync(donationId);
            if (donation != null)
            {
                donation.IsDistributed = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }





    }
}
