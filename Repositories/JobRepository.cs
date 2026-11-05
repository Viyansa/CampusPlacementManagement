using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Repositories;
public class JobRepository : IJobRepository
{
    private readonly ApplicationDbContext _db;
    public JobRepository(ApplicationDbContext db)=>_db=db;
    public Task<List<Job>> GetAllVisibleAsync(int? collegeId=null)
    {
        var q=_db.Jobs.Include(x=>x.Company).Include(x=>x.TargetColleges).Include(x=>x.EligibilityCriteria).Where(x=>x.Status==Models.Enums.JobStatus.Published);
        if(collegeId.HasValue) q=q.Where(x=>x.IsOpenToAllColleges || x.TargetColleges.Any(t=>t.CollegeId==collegeId.Value));
        return q.AsNoTracking().OrderByDescending(x=>x.CreatedAt).ToListAsync();
    }
    public Task<Job?> GetByIdAsync(int id)=>_db.Jobs.Include(x=>x.Company).Include(x=>x.CreatedByHR).ThenInclude(x=>x.User).Include(x=>x.TargetColleges).Include(x=>x.EligibilityCriteria).FirstOrDefaultAsync(x=>x.JobId==id);
    public Task<Job?> GetOwnedByHrAsync(int id,int hrId)=>_db.Jobs.Include(x=>x.TargetColleges).Include(x=>x.EligibilityCriteria).FirstOrDefaultAsync(x=>x.JobId==id&&x.CreatedByHRId==hrId);
    public Task AddAsync(Job job)=>_db.Jobs.AddAsync(job).AsTask();
    public void Update(Job job)=>_db.Jobs.Update(job);
    public void Remove(Job job)=>_db.Jobs.Remove(job);
    public Task SaveChangesAsync()=>_db.SaveChangesAsync();
}
