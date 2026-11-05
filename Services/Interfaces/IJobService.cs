using CampusPlacementManagement.DTOs.Jobs;
using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Services.Interfaces;
public interface IJobService
{
    Task<Job> CreateAsync(CreateJobDto dto,string userId); Task<List<Job>> GetVisibleAsync(string userId); Task<Job> GetByIdAsync(int id,string userId);
    Task UpdateAsync(int id,UpdateJobDto dto,string userId); Task PublishAsync(int id,string userId); Task CloseAsync(int id,string userId); Task DeleteAsync(int id,string userId);
    Task AddTargetCollegeAsync(int jobId,int collegeId,string userId); Task<List<JobTargetCollege>> GetTargetCollegesAsync(int jobId,string userId); Task RemoveTargetCollegeAsync(int jobId,int collegeId,string userId);
}
