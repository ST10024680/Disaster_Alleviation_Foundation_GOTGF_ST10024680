using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Disaster_Alleviation_Foundation_GOTGF_.Pages.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
	public class VolunteerLoginModelTests : IDisposable
	{
		private readonly VolunteerLoginModel _model;
		private readonly UserContext _context;

		public VolunteerLoginModelTests()
		{
			var options = new DbContextOptionsBuilder<UserContext>()
				.UseInMemoryDatabase(databaseName: "VolunteerLoginTestDb")
				.Options;

			_context = new UserContext(options);
			_model = new VolunteerLoginModel(_context);

			// Seed the database with a volunteer for testing
			_context.Volunteer.Add(new Volunteer { VName = "Test Volunteer", Email = "test@example.com", VPhoneNumber = "1234567890" });
			_context.SaveChanges();
		}

		[Fact]
		public async Task OnPostAsync_ValidLogin_ShouldRedirectToMemberPage()
		{
			// Arrange
			_model.LName = "Test Volunteer";
			_model.LEmail = "test@example.com";

			// Act
			var result = await _model.OnPostAsync();

			// Assert
			var redirectResult = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/App/Member", redirectResult.PageName);
		}

		[Fact]
		public async Task OnPostAsync_InvalidLogin_ShouldReturnPageWithErrorMessage()
		{
			// Arrange
			_model.LName = "Invalid Name";
			_model.LEmail = "invalid@example.com";

			// Act
			var result = await _model.OnPostAsync();

			// Assert
			var pageResult = Assert.IsType<PageResult>(result);
			Assert.False(_model.ModelState.IsValid);
			Assert.Contains("Invalid login attempt.", _model.ModelState[""].Errors.Select(e => e.ErrorMessage));
		}

		[Fact]
		public async Task OnPostAsync_EmptyFields_ShouldReturnPageWithErrorMessage()
		{
			// Arrange
			_model.LName = "";
			_model.LEmail = "";

			// Act
			var result = await _model.OnPostAsync();

			// Assert
			var pageResult = Assert.IsType<PageResult>(result);
			Assert.False(_model.ModelState.IsValid);
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}

}
