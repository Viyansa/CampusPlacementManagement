using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Services.Interfaces;
public interface IPlacementOfficerService
{
    Task<PlacementOfficer> RegisterAsync(OfficerRegistrationDto dto);
    Task<List<PlacementOfficer>> GetAllAsync(); Task<List<PlacementOfficer>> GetPendingAsync(); Task<PlacementOfficer> GetByIdAsync(int id);
    Task ApproveAsync(int id,string adminId); Task RejectAsync(int id); Task SuspendAsync(int id); Task<PlacementOfficer> GetMeAsync(string userId);
}
