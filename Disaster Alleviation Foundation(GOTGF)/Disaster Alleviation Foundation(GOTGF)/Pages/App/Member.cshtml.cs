using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Disaster_Alleviation_Foundation_GOTGF_.Pages.App
{
    public class MemberModel : PageModel
    {
        private readonly UserContext _context;

        public MemberModel(UserContext context)
        {
            _context = context;
        }

        // Fetch and display tasks
        public IList<Tasks> Tasks { get; set; } = new List<Tasks>();

        // Input model for task creation
        [BindProperty]
        public string title { get; set; }

        [BindProperty]
        public string description { get; set; }

        [BindProperty]
        public DateTime ScheduledDate { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch tasks from the database
            Tasks = await _context.Tasks.ToListAsync();
        }

        // For task creation
        public async Task<IActionResult> OnPostAsync()
        {
			if (ModelState.IsValid)
			{
				// Create a new task object
				var newTask = new Tasks
                {
                    Title = title,
                    Description = description,
                    ScheduledDate = ScheduledDate
                };

                // Save task to the database
                _context.Tasks.Add(newTask);
                await _context.SaveChangesAsync();

                // Redirect to avoid form resubmission
                return RedirectToPage();
            }

            return Page();
        }

        
    }
}
