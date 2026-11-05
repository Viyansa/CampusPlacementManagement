using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.Interviews;

public class CreateInterviewRoundDto
{
    [Required, MaxLength(100)] public string RoundName { get; set; } = string.Empty;
    [Range(1, 100)] public int RoundNumber { get; set; }
    public string? Description { get; set; }
}

public class CreateInterviewScheduleDto
{
    [Required] public int InterviewRoundId { get; set; }
    [Required] public DateTime ScheduledAt { get; set; }
    [MaxLength(500)] public string? MeetingLink { get; set; }
    [MaxLength(255)] public string? Venue { get; set; }
}

public class UpdateInterviewDto
{
    [Required] public DateTime ScheduledAt { get; set; }
    public string? MeetingLink { get; set; }
    public string? Venue { get; set; }
}

public class InterviewFeedbackDto
{
    [Required] public string Feedback { get; set; } = string.Empty;
}
