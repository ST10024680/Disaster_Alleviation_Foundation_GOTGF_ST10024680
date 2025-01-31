using Moq;
using Disaster_Alleviation_Foundation_GOTGF_.Data;
using Disaster_Alleviation_Foundation_GOTGF_.Models;
using Disaster_Alleviation_Foundation_GOTGF_.Pages.App;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TestProject_Disaster_Alleviation_Foundation_GOTGF_.xUnit_Test
{
	public class ReportingTests
	{
		private Mock<UserContext> _mockContext;
		private Mock<ILogger<ReportingModel>> _mockLogger;
		private ReportingModel _reportingModel;

		public ReportingTests()
		{
			_mockContext = new Mock<UserContext>();
			_mockLogger = new Mock<ILogger<ReportingModel>>();
			_reportingModel = new ReportingModel(_mockContext.Object);
		}

		[Fact]
		public async Task OnGetAsync_ShouldFetchIncidentReports()
		{
			// Arrange
			var incidentReports = new List<IncidentReports>
			{
				new IncidentReports { IncidentReportsID = 1, Description = "Report 1" },
				new IncidentReports { IncidentReportsID = 2, Description = "Report 2" }
			};

			var mockSet = new Mock<DbSet<IncidentReports>>();
			mockSet.As<IQueryable<IncidentReports>>().Setup(m => m.Provider).Returns(incidentReports.AsQueryable().Provider);
			mockSet.As<IQueryable<IncidentReports>>().Setup(m => m.Expression).Returns(incidentReports.AsQueryable().Expression);
			mockSet.As<IQueryable<IncidentReports>>().Setup(m => m.ElementType).Returns(incidentReports.AsQueryable().ElementType);
			mockSet.As<IQueryable<IncidentReports>>().Setup(m => m.GetEnumerator()).Returns(incidentReports.AsQueryable().GetEnumerator());

			_mockContext.Setup(c => c.IncidentReports).Returns(mockSet.Object);

			// Act
			await _reportingModel.OnGetAsync();

			// Assert
			Assert.Equal(2, _reportingModel.IncidentReports.Count);
			Assert.Equal("Report 1", _reportingModel.IncidentReports[0].Description);
		}

		[Fact]
		public async Task OnPostAsync_ShouldAddNewIncidentReport()
		{
			// Arrange
			_reportingModel.NewIncidentReport = new IncidentReports
			{
				IncidentReportsID = 1,
				Description = "New Report"
			};

			var mockSet = new Mock<DbSet<IncidentReports>>();
			_mockContext.Setup(m => m.IncidentReports).Returns(mockSet.Object);
			_mockContext.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

			// Act
			var result = await _reportingModel.OnPostAsync();

			// Assert
			_mockContext.Verify(m => m.IncidentReports.Add(It.IsAny<IncidentReports>()), Times.Once);
			_mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
			Assert.IsType<Microsoft.AspNetCore.Mvc.RedirectToPageResult>(result); // This is the correct usage
		}
	}
}
