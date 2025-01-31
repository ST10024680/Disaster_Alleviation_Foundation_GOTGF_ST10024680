using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Disaster_Alleviation_Foundation_GOTGF_.Pages.Auth
{
    public class AdminLoginModel : PageModel
    {
        private readonly UserContext _context;

        public AdminLoginModel(UserContext context)
        {
            _context = context;
        }
        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string AEmail { get; set; } = default!;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        public string APassword { get; set; } = default!;


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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == AEmail);

            // If user is found, verify the password
            if (user != null && BCrypt.Net.BCrypt.Verify(APassword, user.PasswordHash))
            {
                // Successful login
                return RedirectToPage("/App/Admin");
            }

            // If we get here, the login attempt failed
            ErrorMessage = "Invalid login attempt.";
            return Page(); // Return to the page with an error message
        }
    }
}
