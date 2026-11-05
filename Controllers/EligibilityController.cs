using Microsoft.EntityFrameworkCore;
using System.Security.Claims;using CampusPlacementManagement.Services.Interfaces;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Mvc;
namespace CampusPlacementManagement.Controllers;
[ApiController,Route("api/eligibility"),Authorize]
public class EligibilityController:ControllerBase
{private readonly IEligibilityService _s;private readonly CampusPlacementManagement.Data.ApplicationDbContext _db;public EligibilityController(IEligibilityService s,CampusPlacementManagement.Data.ApplicationDbContext db){_s=s;_db=db;}
[Authorize(Roles="Student"),HttpGet("jobs/{jobId}")]public async Task<IActionResult> Evaluate(int jobId){var student=await _db.Students.FirstOrDefaultAsync(x=>x.UserId==User.FindFirstValue(ClaimTypes.NameIdentifier));if(student==null)return Unauthorized();return Ok(await _s.EvaluateJobForStudentAsync(jobId,student.StudentId));}}
