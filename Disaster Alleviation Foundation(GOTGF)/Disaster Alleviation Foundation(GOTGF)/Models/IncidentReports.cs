namespace Disaster_Alleviation_Foundation_GOTGF_.Models
{
    public class IncidentReports
    {
        public int IncidentReportsID { get; set; }
        public string Location { get; set; }
        public string DisasterType { get; set; }
        public string Severity { get; set; }
        public string Description { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
