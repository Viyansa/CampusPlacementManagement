namespace CampusPlacementManagement.DTOs.Eligibility;

public class EligibilityResultDto
{
    public bool Eligible { get; set; }
    public List<string> Reasons { get; set; } = new();
}
