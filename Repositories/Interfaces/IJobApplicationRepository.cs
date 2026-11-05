using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Repositories.Interfaces;
public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByIdAsync(int id);
    Task<JobApplication?> GetByStudentAndJobAsync(int studentId, int jobId);
    Task<List<JobApplication>> GetByStudentAsync(int studentId);
    Task<List<JobApplication>> GetByJobAsync(int jobId);
    Task AddAsync(JobApplication application);
    void Update(JobApplication application);
    Task SaveChangesAsync();
}
