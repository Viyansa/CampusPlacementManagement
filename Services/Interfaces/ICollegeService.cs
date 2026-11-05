using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Services.Interfaces;
public interface ICollegeService
{
    Task<College> RegisterAsync(CollegeRegisterDto dto);
    Task<List<College>> GetAllAsync(); Task<List<College>> GetPendingAsync(); Task<College> GetByIdAsync(int id);
    Task ApproveAsync(int id,string adminId); Task RejectAsync(int id); Task SuspendAsync(int id); Task UpdateAsync(int id,CollegeRegisterDto dto);
}
