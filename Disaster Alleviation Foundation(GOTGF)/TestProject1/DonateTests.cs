using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Disaster_Alleviation_Foundation_GOTGF_.Pages.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
	public class DonateModelTests
	{
		private readonly DbContextOptions<UserContext> _options;

		public DonateModelTests()
		{
			// Set up an in-memory database for testing
			_options = new DbContextOptionsBuilder<UserContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;
		}

		[Fact]
		public async Task OnPostAsync_ShouldNotAddDonation_WhenModelStateIsInvalid()
		{
			// Arrange
			using var context = new UserContext(_options);
			var model = new DonateModel(context)
			{
				// Intentionally not setting valid properties to make ModelState invalid
				NewResourceDonation = new ResourceDonation
				{
					ResourceType = null, // Invalid input
					Description = "Test donation",
					Quantity = 5
				}
			};
			model.ModelState.AddModelError("ResourceType", "Resource Type is required."); // Simulating a model state error

			// Act
			var result = await model.OnPostAsync();

			// Assert
			Assert.IsType<PageResult>(result); // Should return the same page
			Assert.Empty(await context.ResourceDonation.ToListAsync()); // No donations should be added
		}

		[Fact]
		public async Task OnPostAsync_ShouldAddDonation_WhenModelStateIsValid()
		{
			// Arrange
			using var context = new UserContext(_options);
			var model = new DonateModel(context)
			{
				NewResourceDonation = new ResourceDonation
				{
					ResourceType = "Food",
					Description = "Canned goods",
					Quantity = 10
				}
			};

			// Act
			var result = await model.OnPostAsync();

			// Assert
			//Assert.IsType<RedirectToPageResult>(result); // Should redirect
			Assert.Single(await context.ResourceDonation.ToListAsync()); // One donation should be added
		}
	}
}
