using CampusPlacementManagement.DTOs.Eligibility;
using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Services.Interfaces;
public interface IEligibilityService
{
    Task<EligibilityResultDto> EvaluateAsync(Job job,Student student);
    Task<EligibilityResultDto> EvaluateJobForStudentAsync(int jobId,int studentId);
}
