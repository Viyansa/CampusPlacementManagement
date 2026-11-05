using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
namespace CampusPlacementManagement.Services;
public class CollegeService:ICollegeService
{
    private readonly ICollegeRepository _repo; public CollegeService(ICollegeRepository repo)=>_repo=repo;
    public async Task<College> RegisterAsync(CollegeRegisterDto dto)
    { if(await _repo.GetByCodeAsync(dto.CollegeCode)!=null) throw new InvalidOperationException("College code already exists."); var c=new College{CollegeName=dto.CollegeName,CollegeCode=dto.CollegeCode,UniversityName=dto.UniversityName,OfficialEmail=dto.OfficialEmail,PhoneNumber=dto.PhoneNumber,Address=dto.Address,Website=dto.Website,Status=CollegeStatus.Pending}; await _repo.AddAsync(c); await _repo.SaveChangesAsync(); return c; }
    public Task<List<College>> GetAllAsync()=>_repo.GetAllAsync(); public Task<List<College>> GetPendingAsync()=>_repo.GetPendingAsync();
    public async Task<College> GetByIdAsync(int id)=>await _repo.GetByIdAsync(id)??throw new KeyNotFoundException("College not found.");
    public async Task ApproveAsync(int id,string adminId){var c=await GetByIdAsync(id);c.Status=CollegeStatus.Approved;_repo.Update(c);await _repo.SaveChangesAsync();}
    public async Task RejectAsync(int id){var c=await GetByIdAsync(id);c.Status=CollegeStatus.Rejected;_repo.Update(c);await _repo.SaveChangesAsync();}
    public async Task SuspendAsync(int id){var c=await GetByIdAsync(id);c.Status=CollegeStatus.Suspended;_repo.Update(c);await _repo.SaveChangesAsync();}
    public async Task UpdateAsync(int id,CollegeRegisterDto dto){var c=await GetByIdAsync(id);c.CollegeName=dto.CollegeName;c.UniversityName=dto.UniversityName;c.OfficialEmail=dto.OfficialEmail;c.PhoneNumber=dto.PhoneNumber;c.Address=dto.Address;c.Website=dto.Website;_repo.Update(c);await _repo.SaveChangesAsync();}
}
