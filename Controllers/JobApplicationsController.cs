using System.Security.Claims;using CampusPlacementManagement.DTOs.JobApplications;using CampusPlacementManagement.Services.Interfaces;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace CampusPlacementManagement.Controllers;
[ApiController,Route("api"),Authorize]
public class JobApplicationsController:ControllerBase
{private readonly IJobApplicationService _s;private string Uid()=>User.FindFirstValue(ClaimTypes.NameIdentifier)!;public JobApplicationsController(IJobApplicationService s)=>_s=s;
[Authorize(Roles="Student"),HttpPost("jobs/{jobId}/apply")]public Task<Models.JobApplication> Apply(int jobId,ApplyJobDto d)=>_s.ApplyAsync(jobId,Uid(),d);
[Authorize(Roles="Student"),HttpGet("applications/my")]public Task<List<Models.JobApplication>> My()=>_s.GetMyApplicationsAsync(Uid());
[HttpGet("applications/{id}")]public Task<Models.JobApplication> Get(int id)=>_s.GetByIdAsync(id,Uid());
[Authorize(Roles="CompanyHR"),HttpGet("jobs/{jobId}/applications")]public Task<List<Models.JobApplication>> JobApps(int jobId)=>_s.GetForJobAsync(jobId,Uid());
[Authorize(Roles="CompanyHR"),HttpPut("applications/{id}/shortlist")]public async Task<IActionResult> Shortlist(int id,ApplicationStatusDto d){await _s.ShortlistAsync(id,Uid(),d.Remarks);return Ok();}
[Authorize(Roles="CompanyHR"),HttpPut("applications/{id}/reject")]public async Task<IActionResult> Reject(int id,ApplicationStatusDto d){await _s.RejectAsync(id,Uid(),d.Remarks);return Ok();}
[Authorize(Roles="Student"),HttpPut("applications/{id}/withdraw")]public async Task<IActionResult> Withdraw(int id,ApplicationStatusDto d){await _s.WithdrawAsync(id,Uid(),d.Remarks);return Ok();}}
