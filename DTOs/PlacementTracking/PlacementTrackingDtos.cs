namespace CampusPlacementManagement.DTOs.PlacementTracking;

public class PlacementOverviewDto
{
    public int TotalStudents { get; set; }
    public int TotalApplications { get; set; }
    public int TotalSelected { get; set; }
    public decimal PlacementPercentage { get; set; }
}
