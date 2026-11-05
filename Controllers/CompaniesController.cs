using System.Security.Claims;using CampusPlacementManagement.DTOs.Companies;using CampusPlacementManagement.Services.Interfaces;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace CampusPlacementManagement.Controllers;
[ApiController,Route("api")]
public class CompaniesController:ControllerBase
{private readonly ICompanyService _s;public CompaniesController(ICompanyService s)=>_s=s;
[AllowAnonymous,HttpPost("companies/register")]public Task<Models.Company> Register(CompanyRegistrationDto d)=>_s.RegisterAsync(d);
[AllowAnonymous,HttpPost("company-hrs/register")]public Task<Models.CompanyHR> RegisterHr(HrRegistrationDto d)=>_s.RegisterHrAsync(d);
[Authorize(Roles="SuperAdmin"),HttpGet("companies")]public Task<List<Models.Company>> All()=>_s.GetAllAsync();
[Authorize(Roles="SuperAdmin,CompanyHR"),HttpGet("companies/{id}")]public Task<Models.Company> Get(int id)=>_s.GetByIdAsync(id);
[Authorize(Roles="SuperAdmin"),HttpGet("companies/pending")]public Task<List<Models.Company>> Pending()=>_s.GetPendingAsync();
[Authorize(Roles="SuperAdmin"),HttpPut("companies/{id}/approve")]public async Task<IActionResult> Approve(int id){await _s.ApproveAsync(id);return Ok();}
[Authorize(Roles="SuperAdmin"),HttpPut("companies/{id}/reject")]public async Task<IActionResult> Reject(int id){await _s.RejectAsync(id);return Ok();}
[Authorize(Roles="SuperAdmin"),HttpPut("companies/{id}/suspend")]public async Task<IActionResult> Suspend(int id){await _s.SuspendAsync(id);return Ok();}
[Authorize(Roles="SuperAdmin,CompanyHR"),HttpPut("companies/{id}")]public async Task<IActionResult> Update(int id,CompanyRegistrationDto d){await _s.UpdateAsync(id,d);return Ok();}
[Authorize(Roles="SuperAdmin"),HttpGet("company-hrs/pending")]public Task<List<Models.CompanyHR>> PendingHr()=>_s.GetPendingHrAsync();
[Authorize(Roles="SuperAdmin"),HttpGet("company-hrs")]public Task<List<Models.CompanyHR>> AllHr()=>_s.GetAllHrAsync();
[Authorize(Roles="SuperAdmin"),HttpPut("company-hrs/{id}/approve")]public async Task<IActionResult> ApproveHr(int id){await _s.ApproveHrAsync(id,User.FindFirstValue(ClaimTypes.NameIdentifier)!);return Ok();}
[Authorize(Roles="SuperAdmin"),HttpPut("company-hrs/{id}/reject")]public async Task<IActionResult> RejectHr(int id){await _s.RejectHrAsync(id);return Ok();}
[Authorize(Roles="CompanyHR"),HttpGet("company-hrs/me")]public Task<Models.CompanyHR> Me()=>_s.GetMyHrAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
