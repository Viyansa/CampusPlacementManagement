using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Repositories.Interfaces;
public interface ICompanyRepository
{
    Task<List<Company>> GetAllAsync();
    Task<List<Company>> GetPendingAsync();
    Task<Company?> GetByIdAsync(int id);
    Task AddAsync(Company company);
    void Update(Company company);
    Task SaveChangesAsync();
}
