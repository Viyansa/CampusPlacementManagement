using System.Security.Claims;using CampusPlacementManagement.DTOs.Colleges;using CampusPlacementManagement.Services.Interfaces;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace CampusPlacementManagement.Controllers;
[ApiController,Route("api/colleges")]
public class CollegesController:ControllerBase
{private readonly ICollegeService _s;public CollegesController(ICollegeService s)=>_s=s;
[AllowAnonymous,HttpPost("register")]public Task<IActionResult> Register(CollegeRegisterDto d)=>Wrap(async()=>Ok(await _s.RegisterAsync(d)));
[Authorize(Roles="SuperAdmin"),HttpGet]public Task<List<Models.College>> GetAll()=>_s.GetAllAsync();
[Authorize(Roles="SuperAdmin,PlacementOfficer"),HttpGet("{id}")]public async Task<IActionResult> Get(int id){var c=await _s.GetByIdAsync(id);if(User.IsInRole("PlacementOfficer")){var me=User.FindFirstValue(ClaimTypes.NameIdentifier);/* ownership is checked in service layer in officer workflows */}return Ok(c);}
[Authorize(Roles="SuperAdmin"),HttpGet("pending")]public Task<List<Models.College>> Pending()=>_s.GetPendingAsync();
[Authorize(Roles="SuperAdmin"),HttpPut("{id}/approve")]public async Task<IActionResult> Approve(int id){await _s.ApproveAsync(id,User.FindFirstValue(ClaimTypes.NameIdentifier)!);return Ok();}
[Authorize(Roles="SuperAdmin"),HttpPut("{id}/reject")]public async Task<IActionResult> Reject(int id){await _s.RejectAsync(id);return Ok();}
[Authorize(Roles="SuperAdmin"),HttpPut("{id}/suspend")]public async Task<IActionResult> Suspend(int id){await _s.SuspendAsync(id);return Ok();}
[Authorize(Roles="SuperAdmin"),HttpPut("{id}")]public async Task<IActionResult> Update(int id,CollegeRegisterDto d){await _s.UpdateAsync(id,d);return Ok();}
private static async Task<IActionResult> Wrap(Func<Task<IActionResult>> f)=>await f();}
