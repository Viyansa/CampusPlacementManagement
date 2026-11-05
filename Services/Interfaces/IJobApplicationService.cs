using CampusPlacementManagement.DTOs.JobApplications;
using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Services.Interfaces;
public interface IJobApplicationService
{
    Task<JobApplication> ApplyAsync(int jobId,string studentUserId,ApplyJobDto dto);
    Task<List<JobApplication>> GetMyApplicationsAsync(string userId); Task<JobApplication> GetByIdAsync(int id,string userId);
    Task<List<JobApplication>> GetForJobAsync(int jobId,string userId); Task ShortlistAsync(int id,string userId,string? remarks); Task RejectAsync(int id,string userId,string? remarks); Task WithdrawAsync(int id,string userId,string? remarks);
}
