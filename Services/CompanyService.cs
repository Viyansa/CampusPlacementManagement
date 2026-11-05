using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Companies;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Services;
public class CompanyService:ICompanyService
{
 private readonly ICompanyRepository _repo; private readonly UserManager<ApplicationUser> _users; private readonly ApplicationDbContext _db;
 public CompanyService(ICompanyRepository repo,UserManager<ApplicationUser> users,ApplicationDbContext db){_repo=repo;_users=users;_db=db;}
 public async Task<Company> RegisterAsync(CompanyRegistrationDto dto){if(await _db.Companies.AnyAsync(x=>x.CompanyEmail==dto.CompanyEmail))throw new InvalidOperationException("Company email already exists.");var c=new Company{CompanyName=dto.CompanyName,CompanyEmail=dto.CompanyEmail,Website=dto.Website,Industry=dto.Industry,Address=dto.Address,Description=dto.Description,Status=CompanyStatus.Pending};await _repo.AddAsync(c);await _repo.SaveChangesAsync();return c;}
 public async Task<CompanyHR> RegisterHrAsync(HrRegistrationDto dto){var c=await _repo.GetByIdAsync(dto.CompanyId)??throw new KeyNotFoundException("Company not found.");if(c.Status!=CompanyStatus.Approved)throw new InvalidOperationException("Company HR can register only against an approved company.");if(await _users.FindByEmailAsync(dto.Email)!=null)throw new InvalidOperationException("Email already exists.");var u=new ApplicationUser{UserName=dto.Email,Email=dto.Email,EmailConfirmed=true,PhoneNumber=dto.PhoneNumber,FullName=dto.FullName,AccountStatus=AccountStatus.Pending};var r=await _users.CreateAsync(u,dto.Password);if(!r.Succeeded)throw new InvalidOperationException(string.Join("; ",r.Errors.Select(x=>x.Description)));var hr=new CompanyHR{UserId=u.Id,CompanyId=c.CompanyId,Designation=dto.Designation,Status=AccountStatus.Pending};await _db.CompanyHRs.AddAsync(hr);await _db.SaveChangesAsync();return hr;}
 public Task<List<Company>> GetAllAsync()=>_repo.GetAllAsync();public Task<List<Company>> GetPendingAsync()=>_repo.GetPendingAsync();public async Task<Company> GetByIdAsync(int id)=>await _repo.GetByIdAsync(id)??throw new KeyNotFoundException("Company not found.");
 public async Task ApproveAsync(int id){var c=await GetByIdAsync(id);c.Status=CompanyStatus.Approved;_repo.Update(c);await _repo.SaveChangesAsync();}public async Task RejectAsync(int id){var c=await GetByIdAsync(id);c.Status=CompanyStatus.Rejected;_repo.Update(c);await _repo.SaveChangesAsync();}public async Task SuspendAsync(int id){var c=await GetByIdAsync(id);c.Status=CompanyStatus.Suspended;_repo.Update(c);await _repo.SaveChangesAsync();}public async Task UpdateAsync(int id,CompanyRegistrationDto dto){var c=await GetByIdAsync(id);c.CompanyName=dto.CompanyName;c.CompanyEmail=dto.CompanyEmail;c.Website=dto.Website;c.Industry=dto.Industry;c.Address=dto.Address;c.Description=dto.Description;_repo.Update(c);await _repo.SaveChangesAsync();}
 public Task<List<CompanyHR>> GetPendingHrAsync()=>_db.CompanyHRs.Include(x=>x.User).Include(x=>x.Company).Where(x=>x.Status==AccountStatus.Pending).AsNoTracking().ToListAsync();public Task<List<CompanyHR>> GetAllHrAsync()=>_db.CompanyHRs.Include(x=>x.User).Include(x=>x.Company).AsNoTracking().ToListAsync();
 public async Task ApproveHrAsync(int id,string adminId){var h=await _db.CompanyHRs.Include(x=>x.User).FirstOrDefaultAsync(x=>x.CompanyHRId==id)??throw new KeyNotFoundException("Company HR not found.");h.Status=AccountStatus.Approved;h.ApprovedBy=adminId;h.ApprovedAt=DateTime.UtcNow;h.User.AccountStatus=AccountStatus.Approved;await _users.AddToRoleAsync(h.User,"CompanyHR");await _db.SaveChangesAsync();}
 public async Task RejectHrAsync(int id){var h=await _db.CompanyHRs.Include(x=>x.User).FirstOrDefaultAsync(x=>x.CompanyHRId==id)??throw new KeyNotFoundException("Company HR not found.");h.Status=AccountStatus.Rejected;h.User.AccountStatus=AccountStatus.Rejected;await _db.SaveChangesAsync();}
 public async Task<CompanyHR> GetMyHrAsync(string userId)=>await _db.CompanyHRs.Include(x=>x.Company).FirstOrDefaultAsync(x=>x.UserId==userId)??throw new KeyNotFoundException("Company HR profile not found.");
}
