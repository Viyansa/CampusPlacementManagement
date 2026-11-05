using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Repositories.Interfaces;
public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync(int? collegeId = null, string? department = null, int? graduationYear = null, string? search = null);
    Task<List<Student>> GetPendingAsync(int collegeId);
    Task<Student?> GetByIdAsync(int id);
    Task<Student?> GetByUserIdAsync(string userId);
    Task<bool> RegisterNumberExistsAsync(int collegeId, string registerNumber);
    Task AddAsync(Student student);
    void Update(Student student);
    Task SaveChangesAsync();
}
