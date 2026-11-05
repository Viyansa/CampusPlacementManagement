using System.ComponentModel.DataAnnotations;

namespace CampusPlacementManagement.DTOs.PlacementOfficers;

public class OfficerRegistrationDto
{
    [Required, MaxLength(200)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(8)] public string Password { get; set; } = string.Empty;
    [Required, MaxLength(20)] public string MobileNumber { get; set; } = string.Empty;
    [Required] public int CollegeId { get; set; }
    [Required, MaxLength(100)] public string Designation { get; set; } = string.Empty;
}
