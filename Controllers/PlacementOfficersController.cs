using System.Security.Claims;using CampusPlacementManagement.DTOs.PlacementOfficers;using CampusPlacementManagement.Services.Interfaces;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace CampusPlacementManagement.Controllers;
[ApiController,Route("api/placement-officers")]
public class PlacementOfficersController:ControllerBase
{private readonly IPlacementOfficerService _s;public PlacementOfficersController(IPlacementOfficerService s)=>_s=s;
[AllowAnonymous,HttpPost("register")]public Task<Models.PlacementOfficer> Register(OfficerRegistrationDto d)=>_s.RegisterAsync(d);
[Authorize(Roles="SuperAdmin"),HttpGet("pending")]public Task<List<Models.PlacementOfficer>> Pending()=>_s.GetPendingAsync();
[Authorize(Roles="SuperAdmin"),HttpGet]public Task<List<Models.PlacementOfficer>> All()=>_s.GetAllAsync();
[Authorize(Roles="SuperAdmin,PlacementOfficer"),HttpGet("{id}")]public async Task<IActionResult> Get(int id)=>Ok(await _s.GetByIdAsync(id));
[Authorize(Roles="SuperAdmin"),HttpPut("{id}/approve")]public async Task<IActionResult> Approve(int id){await _s.ApproveAsync(id,User.FindFirstValue(ClaimTypes.NameIdentifier)!);return Ok();}
[Authorize(Roles="SuperAdmin"),HttpPut("{id}/reject")]public async Task<IActionResult> Reject(int id){await _s.RejectAsync(id);return Ok();}
[Authorize(Roles="SuperAdmin"),HttpPut("{id}/suspend")]public async Task<IActionResult> Suspend(int id){await _s.SuspendAsync(id);return Ok();}
[Authorize(Roles="PlacementOfficer"),HttpGet("me")]public Task<Models.PlacementOfficer> Me()=>_s.GetMeAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
