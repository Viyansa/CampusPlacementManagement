namespace CampusPlacementManagement.Models;

public class EligibilityCriteria
{
    public int EligibilityCriteriaId { get; set; }
    public int JobId { get; set; }
    public decimal? MinimumCGPA { get; set; }
    public int? MaximumBacklogs { get; set; }
    public string? Department { get; set; }
    public int? GraduationYear { get; set; }

    public Job Job { get; set; } = null!;
}
