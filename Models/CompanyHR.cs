using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models;

public class CompanyHR
{
    public int CompanyHRId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public string? Designation { get; set; }
    public AccountStatus Status { get; set; } = AccountStatus.Pending;
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public Company Company { get; set; } = null!;
    public ApplicationUser? Approver { get; set; }
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
