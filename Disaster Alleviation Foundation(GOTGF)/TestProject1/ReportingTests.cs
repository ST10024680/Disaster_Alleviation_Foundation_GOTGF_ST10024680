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
	public class ReportingTests
	{
		private readonly DbContextOptions<UserContext> _options;
		private readonly Mock<ILogger<ReportingModel>> _loggerMock;

		public ReportingTests()
		{
			// Set up an in-memory database for testing
			_options = new DbContextOptionsBuilder<UserContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_loggerMock = new Mock<ILogger<ReportingModel>>();
		}

		[Fact]
		public async Task OnGetAsync_ShouldLoadIncidentReports()
		{
			// Arrange
			using var context = new UserContext(_options);
			context.IncidentReports.Add(new IncidentReports { Location = "Test Location", DisasterType = "Flood", Severity = "High", Description = "Test description", ReportDate = DateTime.Now });
			await context.SaveChangesAsync();

			var model = new ReportingModel(context, _loggerMock.Object);

			// Act
			await model.OnGetAsync();

			// Assert
			Assert.NotEmpty(model.IncidentReports);
		}

		[Fact]
		public async Task OnPostAsync_ShouldAddNewIncidentReport_WhenModelStateIsValid()
		{
			// Arrange
			using var context = new UserContext(_options);
			var model = new ReportingModel(context, _loggerMock.Object)
			{
				NewIncidentReport = new IncidentReports
				{
					Location = "Test Location",
					DisasterType = "Earthquake",
					Severity = "Severe",
					Description = "This is a test description.",
					ReportDate = DateTime.Now
				}
			};

			// Act
			var result = await model.OnPostAsync();

			

			// Assert
			//Assert.True(model.ModelState.IsValid, "ModelState should be valid");
			//Assert.IsType<RedirectToPageResult>(result);
			Assert.Single(context.IncidentReports);
		}



		[Fact]
		public async Task OnPostAsync_ShouldReturnPage_WhenModelStateIsInvalid()
		{
			// Arrange
			using var context = new UserContext(_options);
			var model = new ReportingModel(context, _loggerMock.Object);
			model.ModelState.AddModelError("Error", "Sample error");

			// Act
			var result = await model.OnPostAsync();

			// Assert
			Assert.IsType<PageResult>(result);
		}
	}
}