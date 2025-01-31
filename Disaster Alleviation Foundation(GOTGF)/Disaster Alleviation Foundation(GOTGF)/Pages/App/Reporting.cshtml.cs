using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace Disaster_Alleviation_Foundation_GOTGF_.Pages.App
{
    public class ReportingModel : PageModel
    {
        private readonly UserContext _context;
		private readonly ILogger<ReportingModel> _logger;

		public ReportingModel(UserContext context, ILogger<ReportingModel> logger)
        {
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger;
		}

        public IList<IncidentReports> IncidentReports { get; set; } = new List<IncidentReports>();

        // This property will hold the new incident report data
        [BindProperty]
        public IncidentReports NewIncidentReport { get; set; } = new IncidentReports();

        public async Task OnGetAsync()
        {
            try
            {
                // Fetch incident reports from the database
                IncidentReports = await _context.IncidentReports.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error (consider using a logging framework)
                ModelState.AddModelError(string.Empty, "Unable to load incident reports. Please try again later.");
                Console.WriteLine($"Error loading incident reports: {ex.Message}"); // Logging for debugging
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Return the page with validation errors if any
                return Page();
            }

            try
            {
                // Set the report date to current date
                NewIncidentReport.ReportDate = DateTime.Now;

                // Save the new incident report to the database
                _context.IncidentReports.Add(NewIncidentReport);
                await _context.SaveChangesAsync();

                // Show a success message
                TempData["SuccessMessage"] = "Incident report submitted successfully!";
                return RedirectToPage(); // Redirect to the same page to refresh the report list
            }
            catch (Exception ex)
            {
                // Log the error 
                ModelState.AddModelError(string.Empty, "Unable to save the report. Please try again later.");
                Console.WriteLine($"Error saving incident report: {ex.Message}");
                return Page();
            }
        }

    }
}
