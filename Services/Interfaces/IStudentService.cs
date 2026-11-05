using CampusPlacementManagement.DTOs.Students;
using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Services.Interfaces;
public interface IStudentService
{
    Task<Student> EnrollAsync(StudentEnrollmentDto dto,string officerUserId);
    Task<BulkImportResultDto> BulkEnrollAsync(Stream file,string fileName,string officerUserId);
    Task<List<Student>> GetAllAsync(string userId,string? department,int? graduationYear,string? search);
    Task<List<Student>> GetPendingAsync(string userId); Task<Student> GetByIdAsync(int id); Task<Student> GetMeAsync(string userId);
    Task ApproveAsync(int id,string officerUserId); Task RejectAsync(int id,string officerUserId); Task SuspendAsync(int id,string userId); Task UpdateAsync(int id,StudentEnrollmentDto dto,string userId);
}
