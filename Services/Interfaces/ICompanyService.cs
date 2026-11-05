using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Services.Interfaces;
public interface ICompanyService
{
    Task<Company> RegisterAsync(CompanyRegistrationDto dto); Task<CompanyHR> RegisterHrAsync(HrRegistrationDto dto);
    Task<List<Company>> GetAllAsync(); Task<List<Company>> GetPendingAsync(); Task<Company> GetByIdAsync(int id);
    Task ApproveAsync(int id); Task RejectAsync(int id); Task SuspendAsync(int id); Task UpdateAsync(int id,CompanyRegistrationDto dto);
    Task<List<CompanyHR>> GetPendingHrAsync(); Task<List<CompanyHR>> GetAllHrAsync(); Task ApproveHrAsync(int id,string adminId); Task RejectHrAsync(int id); Task<CompanyHR> GetMyHrAsync(string userId);
}
