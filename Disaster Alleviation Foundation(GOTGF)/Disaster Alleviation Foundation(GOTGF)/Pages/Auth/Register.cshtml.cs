using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Disaster_Alleviation_Foundation_GOTGF_.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly UserContext _context;

        public RegisterModel(UserContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [BindProperty]
        [Required]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string SuccessMessage { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

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

			// Check if the email already exists
			var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
			if (existingUser != null)
			{
				ModelState.AddModelError(string.Empty, "This email address is already registered.");
				return Page();
			}

			if (Password != ConfirmPassword)
			{
				ModelState.AddModelError(string.Empty, "The password and confirmation password do not match.");
				return Page();
			}

			try
			{
				// Hash the password
				var passwordHash = BCrypt.Net.BCrypt.HashPassword(Password);

				// Create the new user
				var newUser = new Users
				{
					Name = Name,
					Email = Email,
					PasswordHash = passwordHash
				};

				// Add the user to the database
				_context.Users.Add(newUser);
				await _context.SaveChangesAsync();

				SuccessMessage = "Registration successful! Please log in.";
				return RedirectToPage("/Auth/Login");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, "An error occurred while registering. Please try again.");
				Console.WriteLine($"Error during registration: {ex.Message}"); // Log error for debugging
				return Page();
			}
		}

	}
}
