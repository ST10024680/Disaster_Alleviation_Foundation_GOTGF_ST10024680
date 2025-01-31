using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Disaster_Alleviation_Foundation_GOTGF_.Pages.App;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
	public class MemberTest
	{
		private readonly DbContextOptions<UserContext> _options;

		public MemberTest()
		{
			// Set up an in-memory database for testing
			_options = new DbContextOptionsBuilder<UserContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;
		}

		[Fact]
		public async Task OnPostAsync_ShouldAddNewTask_WhenModelStateIsValid()
		{
			// Arrange
			using var context = new UserContext(_options);
			var model = new MemberModel(context)
			{
				title = "Test Task",
				description = "This is a test task description",
				ScheduledDate = DateTime.Today
			};

			// Act
			var result = await model.OnPostAsync();

			// Assert
			Assert.True(model.ModelState.IsValid, "ModelState should be valid");
			Assert.Single(context.Tasks); // Check if a task has been added to the database
		}

		[Fact]
		public async Task OnPostAsync_ShouldNotAddTask_WhenModelStateIsInvalid()
		{
			// Arrange
			using var context = new UserContext(_options);
			var model = new MemberModel(context);

			// Setting the ModelState to invalid
			model.ModelState.AddModelError("Error", "Invalid input");

			// Act
			var result = await model.OnPostAsync();

			// Assert
			Assert.False(model.ModelState.IsValid, "ModelState should be invalid");
			Assert.Empty(context.Tasks); // Ensure no task has been added to the database
		}

		[Fact]
		public async Task OnGetAsync_ShouldLoadTasksFromDatabase()
		{
			// Arrange
			using var context = new UserContext(_options);
			context.Tasks.Add(new Tasks
			{
				Title = "Existing Task",
				Description = "An existing task in the database",
				ScheduledDate = DateTime.Today
			});
			await context.SaveChangesAsync();

			var model = new MemberModel(context);

			// Act
			await model.OnGetAsync();

			// Assert
			Assert.NotEmpty(model.Tasks); // Check that tasks are loaded from the database
		}
	}
}