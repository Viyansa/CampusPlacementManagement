namespace CampusPlacementManagement.Models;

public class PlacementStatistic
{
    public int PlacementStatisticId { get; set; }
    public int? CollegeId { get; set; }
    public int? CompanyId { get; set; }
    public int GraduationYear { get; set; }
    public int TotalStudents { get; set; }
    public int TotalApplications { get; set; }
    public int TotalSelected { get; set; }
    public decimal PlacementPercentage { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public College? College { get; set; }
    public Company? Company { get; set; }
}
