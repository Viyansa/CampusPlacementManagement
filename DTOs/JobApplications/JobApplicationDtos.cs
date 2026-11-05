using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.JobApplications;

public class ApplyJobDto
{
    public string? ResumeUrl { get; set; }
}

public class ApplicationStatusDto
{
    [MaxLength(500)] public string? Remarks { get; set; }
}
