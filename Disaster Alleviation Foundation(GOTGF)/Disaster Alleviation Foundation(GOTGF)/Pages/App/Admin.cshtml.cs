using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;

namespace Disaster_Alleviation_Foundation_GOTGF_.Pages.App
{
    public class AdminModel : PageModel
    {
        private readonly UserContext _context;

        public AdminModel(UserContext context)
        {
            _context = context;
        }

        public IList<Users> Users { get; set; } = new List<Users>();
        public IList<IncidentReports> IncidentReports { get; set; } = new List<IncidentReports>();
        public IList<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
        public IList<Tasks> Tasks { get; set; } = new List<Tasks>();

        public async Task OnGetAsync()
        {
            // Fetch data from the database
            Users = await _context.Users.ToListAsync();
            IncidentReports = await _context.IncidentReports.ToListAsync();
            Volunteers = await _context.Volunteer.ToListAsync();
            Tasks = await _context.Tasks.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDeleteVolunteerAsync(int id)
        {
            var volunteer = await _context.Volunteer.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }

            _context.Volunteer.Remove(volunteer);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostDeleteIncidentReportAsync(int id)
        {
            var report = await _context.IncidentReports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.IncidentReports.Remove(report);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
