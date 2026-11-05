using System.Security.Claims;using CampusPlacementManagement.DTOs.Jobs;using CampusPlacementManagement.Services.Interfaces;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace CampusPlacementManagement.Controllers;
[ApiController,Route("api/jobs"),Authorize]
public class JobsController:ControllerBase
{private readonly IJobService _s;private string Uid()=>User.FindFirstValue(ClaimTypes.NameIdentifier)!;public JobsController(IJobService s)=>_s=s;
[Authorize(Roles="CompanyHR"),HttpPost]public Task<Models.Job> Create(CreateJobDto d)=>_s.CreateAsync(d,Uid());
[HttpGet]public Task<List<Models.Job>> All()=>_s.GetVisibleAsync(Uid());
[HttpGet("{id}")]public Task<Models.Job> Get(int id)=>_s.GetByIdAsync(id,Uid());
[Authorize(Roles="CompanyHR"),HttpPut("{id}")]public async Task<IActionResult> Update(int id,UpdateJobDto d){await _s.UpdateAsync(id,d,Uid());return Ok();}
[Authorize(Roles="CompanyHR"),HttpPut("{id}/publish")]public async Task<IActionResult> Publish(int id){await _s.PublishAsync(id,Uid());return Ok();}
[Authorize(Roles="CompanyHR"),HttpPut("{id}/close")]public async Task<IActionResult> Close(int id){await _s.CloseAsync(id,Uid());return Ok();}
[Authorize(Roles="CompanyHR"),HttpDelete("{id}")]public async Task<IActionResult> Delete(int id){await _s.DeleteAsync(id,Uid());return NoContent();}
[Authorize(Roles="Student"),HttpGet("eligible")]public async Task<IActionResult> Eligible(){var jobs=await _s.GetVisibleAsync(Uid());return Ok(jobs);}
[Authorize(Roles="Student"),HttpGet("{id}/eligibility")]public async Task<IActionResult> Eligibility(int id){var j=await _s.GetByIdAsync(id,Uid());return Ok(new{jobId=j.JobId,minimumCGPA=j.MinimumCGPA,maximumBacklogs=j.MaximumBacklogs,eligibilityRules=j.EligibilityCriteria});}
[Authorize(Roles="CompanyHR"),HttpPost("{id}/target-colleges")]public async Task<IActionResult> Target(int id,TargetCollegeDto d){await _s.AddTargetCollegeAsync(id,d.CollegeId,Uid());return Ok();}
[Authorize(Roles="CompanyHR"),HttpGet("{id}/target-colleges")]public Task<List<Models.JobTargetCollege>> Targets(int id)=>_s.GetTargetCollegesAsync(id,Uid());
[Authorize(Roles="CompanyHR"),HttpDelete("{id}/target-colleges/{collegeId}")]public async Task<IActionResult> RemoveTarget(int id,int collegeId){await _s.RemoveTargetCollegeAsync(id,collegeId,Uid());return NoContent();}
}
