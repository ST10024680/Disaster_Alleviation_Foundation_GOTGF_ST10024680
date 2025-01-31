using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Disaster_Alleviation_Foundation_GOTGF_.Pages.Auth;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModelTests
{
	[Fact]
	public async Task OnPostAsync_ValidInput_ShouldRedirectToLogin()
	{
		// Arrange
		var options = new DbContextOptionsBuilder<UserContext>()
			.UseInMemoryDatabase(databaseName: "UserRegistrationTestDb")
			.Options;

		using (var context = new UserContext(options))
		{
			var model = new RegisterModel(context)
			{
				Name = "Test User",
				Email = "testuser@example.com",
				Password = "Password123",
				ConfirmPassword = "Password123"
			};

			// Act
			var result = await model.OnPostAsync();

			// Assert
			var redirectResult = Assert.IsType<RedirectToPageResult>(result);
			Assert.Equal("/Auth/Login", redirectResult.PageName);
		}
	}

	[Fact]
	public async Task OnPostAsync_EmailAlreadyExists_ShouldReturnToPageWithError()
	{
		// Arrange
		var options = new DbContextOptionsBuilder<UserContext>()
			.UseInMemoryDatabase(databaseName: "UserRegistrationTestDb")
			.Options;

		using (var context = new UserContext(options))
		{
			// Seed the database with a user
			context.Users.Add(new Users
			{
				Name = "Existing User",
				Email = "testuser@example.com",
				PasswordHash = "hashedPassword"
			});
			context.SaveChanges();

			var model = new RegisterModel(context)
			{
				Name = "New User",
				Email = "testuser@example.com",
				Password = "Password123"
			};

			// Act
			var result = await model.OnPostAsync();

			// Assert
			var pageResult = Assert.IsType<PageResult>(result);
			//Assert.True(model.ModelState.IsValid);
			Assert.Single(model.ModelState[""].Errors);
			//Assert.Equal("This email address is already registered.", model.ModelState[""].Errors[0]);
		}
	}

	[Fact]
	public async Task OnPostAsync_PasswordsDoNotMatch_ShouldReturnToPageWithError()
	{
		// Arrange
		var options = new DbContextOptionsBuilder<UserContext>()
			.UseInMemoryDatabase(databaseName: "UserRegistrationTestDb")
			.Options;

		using (var context = new UserContext(options))
		{
			var model = new RegisterModel(context)
			{
				Name = "Test User",
				Email = "testuser@example.com",
				Password = "Password123",
				ConfirmPassword = "DifferentPassword123"
			};

			// Act
			var result = await model.OnPostAsync();

			// Assert
			var pageResult = Assert.IsType<PageResult>(result);
			Assert.False(model.ModelState.IsValid);
			Assert.Single(model.ModelState[""].Errors);
			//Assert.Equal("The password and confirmation password do not match.", model.ModelState[""].Errors[0]);
		}
	}
}
