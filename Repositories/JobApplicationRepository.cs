using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Repositories;
public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly ApplicationDbContext _db;
    public JobApplicationRepository(ApplicationDbContext db)=>_db=db;
    public Task<JobApplication?> GetByIdAsync(int id)=>_db.JobApplications.Include(x=>x.Job).ThenInclude(x=>x.Company).Include(x=>x.Student).ThenInclude(x=>x.College).Include(x=>x.StatusHistory).FirstOrDefaultAsync(x=>x.ApplicationId==id);
    public Task<JobApplication?> GetByStudentAndJobAsync(int studentId,int jobId)=>_db.JobApplications.Include(x=>x.Job).Include(x=>x.Student).FirstOrDefaultAsync(x=>x.StudentId==studentId&&x.JobId==jobId);
    public Task<List<JobApplication>> GetByStudentAsync(int studentId)=>_db.JobApplications.Include(x=>x.Job).ThenInclude(x=>x.Company).Where(x=>x.StudentId==studentId).AsNoTracking().OrderByDescending(x=>x.AppliedAt).ToListAsync();
    public Task<List<JobApplication>> GetByJobAsync(int jobId)=>_db.JobApplications.Include(x=>x.Student).ThenInclude(x=>x.User).Include(x=>x.Student).ThenInclude(x=>x.College).Where(x=>x.JobId==jobId).AsNoTracking().ToListAsync();
    public Task AddAsync(JobApplication application)=>_db.JobApplications.AddAsync(application).AsTask();
    public void Update(JobApplication application)=>_db.JobApplications.Update(application);
    public Task SaveChangesAsync()=>_db.SaveChangesAsync();
}
