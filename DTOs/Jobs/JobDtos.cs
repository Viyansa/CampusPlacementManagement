using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Jobs;

public class CreateJobDto
{
    [Required, MaxLength(200)] public string JobTitle { get; set; } = string.Empty;
    [Required] public string JobDescription { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string JobType { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string Location { get; set; } = string.Empty;
    [Range(0, 100000000)] public decimal? SalaryPackage { get; set; }
    [Range(0, 10)] public decimal? MinimumCGPA { get; set; }
    [Range(0, 100)] public int? MaximumBacklogs { get; set; }
    [Required] public DateTime ApplicationDeadline { get; set; }
    public bool IsOpenToAllColleges { get; set; }
    public List<EligibilityRuleDto> EligibilityRules { get; set; } = new();
}

public class UpdateJobDto : CreateJobDto { }

public class EligibilityRuleDto
{
    [Range(0, 10)] public decimal? MinimumCGPA { get; set; }
    [Range(0, 100)] public int? MaximumBacklogs { get; set; }
    public string? Department { get; set; }
    public int? GraduationYear { get; set; }
}

public class TargetCollegeDto
{
    [Required] public int CollegeId { get; set; }
}
