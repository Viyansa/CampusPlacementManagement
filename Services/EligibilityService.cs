using CampusPlacementManagement.Data;
using CampusPlacementManagement.DTOs.Eligibility;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Services;
public class EligibilityService:IEligibilityService
{
 private readonly ApplicationDbContext _db;public EligibilityService(ApplicationDbContext db)=>_db=db;
 public Task<EligibilityResultDto> EvaluateAsync(Job job,Student student){var reasons=new List<string>();if(student.Status!=Models.Enums.AccountStatus.Approved)reasons.Add("Student account is not approved.");if(job.MinimumCGPA.HasValue&&student.CGPA<job.MinimumCGPA.Value)reasons.Add($"CGPA must be at least {job.MinimumCGPA.Value}.");if(job.MaximumBacklogs.HasValue&&student.ActiveBacklogs>job.MaximumBacklogs.Value)reasons.Add($"Active backlogs must not exceed {job.MaximumBacklogs.Value}.");var rules=job.EligibilityCriteria.ToList();if(rules.Count>0){var matching=rules.Any(r=>(!r.MinimumCGPA.HasValue||student.CGPA>=r.MinimumCGPA.Value)&&(!r.MaximumBacklogs.HasValue||student.ActiveBacklogs<=r.MaximumBacklogs.Value)&&(string.IsNullOrWhiteSpace(r.Department)||string.Equals(r.Department,student.Department,StringComparison.OrdinalIgnoreCase))&&(!r.GraduationYear.HasValue||r.GraduationYear==student.GraduationYear));if(!matching)reasons.Add("Student does not match the configured eligibility rules.");}return Task.FromResult(new EligibilityResultDto{Eligible=reasons.Count==0,Reasons=reasons});}
 public async Task<EligibilityResultDto> EvaluateJobForStudentAsync(int jobId,int studentId){var job=await _db.Jobs.Include(x=>x.EligibilityCriteria).FirstOrDefaultAsync(x=>x.JobId==jobId)??throw new KeyNotFoundException("Job not found.");var student=await _db.Students.FirstOrDefaultAsync(x=>x.StudentId==studentId)??throw new KeyNotFoundException("Student not found.");return await EvaluateAsync(job,student);}
}
