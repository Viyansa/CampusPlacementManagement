using CampusPlacementManagement.DTOs.PlacementOfficers;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace CampusPlacementManagement.Services;
public class PlacementOfficerService:IPlacementOfficerService
{
 private readonly IPlacementOfficerRepository _repo; private readonly ICollegeRepository _colleges; private readonly UserManager<ApplicationUser> _users; private readonly RoleManager<IdentityRole> _roles;
 public PlacementOfficerService(IPlacementOfficerRepository repo,ICollegeRepository colleges,UserManager<ApplicationUser> users,RoleManager<IdentityRole> roles){_repo=repo;_colleges=colleges;_users=users;_roles=roles;}
 public async Task<PlacementOfficer> RegisterAsync(OfficerRegistrationDto dto){var c=await _colleges.GetByIdAsync(dto.CollegeId)??throw new KeyNotFoundException("College not found.");if(c.Status!=CollegeStatus.Approved)throw new InvalidOperationException("Officer can register only against an approved college.");if(await _users.FindByEmailAsync(dto.Email)!=null)throw new InvalidOperationException("Email already exists.");var u=new ApplicationUser{UserName=dto.Email,Email=dto.Email,EmailConfirmed=true,PhoneNumber=dto.MobileNumber,FullName=dto.FullName,AccountStatus=AccountStatus.Pending};var r=await _users.CreateAsync(u,dto.Password);if(!r.Succeeded)throw new InvalidOperationException(string.Join("; ",r.Errors.Select(x=>x.Description)));var o=new PlacementOfficer{UserId=u.Id,CollegeId=c.CollegeId,Designation=dto.Designation,Status=AccountStatus.Pending};await _repo.AddAsync(o);await _repo.SaveChangesAsync();return o;}
 public Task<List<PlacementOfficer>> GetAllAsync()=>_repo.GetAllAsync();public Task<List<PlacementOfficer>> GetPendingAsync()=>_repo.GetPendingAsync();public async Task<PlacementOfficer> GetByIdAsync(int id)=>await _repo.GetByIdAsync(id)??throw new KeyNotFoundException("Placement officer not found.");
 public async Task ApproveAsync(int id,string adminId){var o=await GetByIdAsync(id);o.Status=AccountStatus.Approved;o.ApprovedBy=adminId;o.ApprovedAt=DateTime.UtcNow;o.User.AccountStatus=AccountStatus.Approved;await _users.AddToRoleAsync(o.User,"PlacementOfficer");_repo.Update(o);await _repo.SaveChangesAsync();}
 public async Task RejectAsync(int id){var o=await GetByIdAsync(id);o.Status=AccountStatus.Rejected;o.User.AccountStatus=AccountStatus.Rejected;_repo.Update(o);await _repo.SaveChangesAsync();}
 public async Task SuspendAsync(int id){var o=await GetByIdAsync(id);o.Status=AccountStatus.Suspended;o.User.AccountStatus=AccountStatus.Suspended;_repo.Update(o);await _repo.SaveChangesAsync();}
 public async Task<PlacementOfficer> GetMeAsync(string userId)=>await _repo.GetByUserIdAsync(userId)??throw new KeyNotFoundException("Placement officer profile not found.");
}
