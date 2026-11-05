using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Repositories.Interfaces;
public interface ICollegeRepository
{
    Task<List<College>> GetAllAsync();
    Task<List<College>> GetPendingAsync();
    Task<College?> GetByIdAsync(int id);
    Task<College?> GetByCodeAsync(string code);
    Task AddAsync(College college);
    void Update(College college);
    Task SaveChangesAsync();
}
