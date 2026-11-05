using System.Security.Claims;using CampusPlacementManagement.DTOs.Students;using CampusPlacementManagement.Services.Interfaces;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace CampusPlacementManagement.Controllers;
[ApiController,Route("api/students")]
public class StudentsController:ControllerBase
{private readonly IStudentService _s;public StudentsController(IStudentService s)=>_s=s;
[Authorize(Roles="PlacementOfficer"),HttpPost("enroll")]public Task<Models.Student> Enroll(StudentEnrollmentDto d)=>_s.EnrollAsync(d,Uid());
[Authorize(Roles="PlacementOfficer"),HttpPost("bulk-enroll")]public Task<DTOs.Students.BulkImportResultDto> Bulk(IFormFile file)=>_s.BulkEnrollAsync(file.OpenReadStream(),file.FileName,Uid());
[Authorize(Roles="SuperAdmin,PlacementOfficer"),HttpGet]public Task<List<Models.Student>> All([FromQuery]string? department,[FromQuery]int? graduationYear,[FromQuery]string? search)=>_s.GetAllAsync(Uid(),department,graduationYear,search);
[Authorize(Roles="PlacementOfficer"),HttpGet("pending")]public Task<List<Models.Student>> Pending()=>_s.GetPendingAsync(Uid());
[Authorize(Roles="SuperAdmin,PlacementOfficer,Student"),HttpGet("{id}")]public Task<Models.Student> Get(int id)=>_s.GetByIdAsync(id);
[Authorize(Roles="PlacementOfficer"),HttpPut("{id}/approve")]public async Task<IActionResult> Approve(int id){await _s.ApproveAsync(id,Uid());return Ok();}
[Authorize(Roles="PlacementOfficer"),HttpPut("{id}/reject")]public async Task<IActionResult> Reject(int id){await _s.RejectAsync(id,Uid());return Ok();}
[Authorize(Roles="SuperAdmin,PlacementOfficer"),HttpPut("{id}/suspend")]public async Task<IActionResult> Suspend(int id){await _s.SuspendAsync(id,Uid());return Ok();}
[Authorize(Roles="SuperAdmin,PlacementOfficer,Student"),HttpPut("{id}")]public async Task<IActionResult> Update(int id,StudentEnrollmentDto d){await _s.UpdateAsync(id,d,Uid());return Ok();}
[Authorize(Roles="Student"),HttpGet("me")]public Task<Models.Student> Me()=>_s.GetMeAsync(Uid());
private string Uid()=>User.FindFirstValue(ClaimTypes.NameIdentifier)!;}
