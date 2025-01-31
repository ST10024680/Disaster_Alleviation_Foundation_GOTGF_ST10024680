using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;

namespace Disaster_Alleviation_Foundation_GOTGF_.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly UserContext _context;

        public LoginModel(UserContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; } = default!;

        [BindProperty]
        public string Password { get; set; } = default!;

        public string ErrorMessage { get; set; } = "";

        public IActionResult OnGet()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            // If user is found, verify the password
            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                // Successful login
                return RedirectToPage("/App/Home");
            }

            // If we get here, the login attempt failed
            ErrorMessage = "Invalid login attempt.";
            return Page(); // Return to the page with an error message
        }

    }
}
