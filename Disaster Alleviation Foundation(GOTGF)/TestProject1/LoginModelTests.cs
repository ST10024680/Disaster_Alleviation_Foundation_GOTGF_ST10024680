using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Disaster_Alleviation_Foundation_GOTGF_.Pages.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
	public class LoginModelTests
	{
		private readonly DbContextOptions<UserContext> _options;

		public LoginModelTests()
		{
			// Set up an in-memory database for testing
			_options = new DbContextOptionsBuilder<UserContext>()
				.UseInMemoryDatabase(databaseName: "TestLoginDatabase")
				.Options;

			// Seed the database with a user
			using var context = new UserContext(_options);
			var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password123");
			context.Users.Add(new Users
			{
				Name = "Test User",
				Email = "testuser@example.com",
				PasswordHash = hashedPassword
			});
			context.SaveChanges();
		}

		[Fact]
		public async Task OnPostAsync_ShouldRedirect_WhenLoginIsSuccessful()
		{
			// Arrange
			using var context = new UserContext(_options);
			var model = new LoginModel(context)
			{
				Email = "testuser@example.com",
				Password = "Password123"
			};

			// Act
			var result = await model.OnPostAsync();

			// Assert
			var redirectResult = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/App/Home", redirectResult.PageName); // Check that it redirects to Home
		}

		[Fact]
		public async Task OnPostAsync_ShouldReturnPage_WhenLoginFails()
		{
			// Arrange
			using var context = new UserContext(_options);
			var model = new LoginModel(context)
			{
				Email = "testuser@example.com",
				Password = "WrongPassword" // Incorrect password
			};

			// Act
			var result = await model.OnPostAsync();

			// Assert
			var pageResult = Assert.IsType<PageResult>(result);
			Assert.Equal("Invalid login attempt.", model.ErrorMessage); // Check for the error message
		}

		[Fact]
		public async Task OnPostAsync_ShouldReturnPage_WhenUserNotFound()
		{
			// Arrange
			using var context = new UserContext(_options);
			var model = new LoginModel(context)
			{
				Email = "nonexistent@example.com", // Email not in database
				Password = "Password123"
			};

			// Act
			var result = await model.OnPostAsync();

			// Assert
			var pageResult = Assert.IsType<PageResult>(result);
			Assert.Equal("Invalid login attempt.", model.ErrorMessage); // Check for the error message
		}
	}
}
