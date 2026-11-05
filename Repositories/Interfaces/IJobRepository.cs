using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Repositories.Interfaces;
public interface IJobRepository
{
    Task<List<Job>> GetAllVisibleAsync(int? collegeId = null);
    Task<Job?> GetByIdAsync(int id);
    Task<Job?> GetOwnedByHrAsync(int id, int hrId);
    Task AddAsync(Job job);
    void Update(Job job);
    void Remove(Job job);
    Task SaveChangesAsync();
}
