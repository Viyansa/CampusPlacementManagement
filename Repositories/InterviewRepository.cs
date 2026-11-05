using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Repositories;
public class InterviewRepository : IInterviewRepository
{
    private readonly ApplicationDbContext _db;
    public InterviewRepository(ApplicationDbContext db)=>_db=db;
    public Task<InterviewRound?> GetRoundByIdAsync(int id)=>_db.InterviewRounds.Include(x=>x.Job).FirstOrDefaultAsync(x=>x.InterviewRoundId==id);
    public Task<List<InterviewRound>> GetRoundsByJobAsync(int jobId)=>_db.InterviewRounds.Include(x=>x.Schedules).Where(x=>x.JobId==jobId).OrderBy(x=>x.RoundNumber).AsNoTracking().ToListAsync();
    public Task<InterviewSchedule?> GetScheduleByIdAsync(int id)=>_db.InterviewSchedules.Include(x=>x.Application).ThenInclude(x=>x.Job).Include(x=>x.InterviewRound).FirstOrDefaultAsync(x=>x.InterviewScheduleId==id);
    public Task<List<InterviewSchedule>> GetSchedulesByApplicationAsync(int applicationId)=>_db.InterviewSchedules.Include(x=>x.InterviewRound).Where(x=>x.ApplicationId==applicationId).AsNoTracking().OrderBy(x=>x.ScheduledAt).ToListAsync();
    public Task AddRoundAsync(InterviewRound round)=>_db.InterviewRounds.AddAsync(round).AsTask();
    public Task AddScheduleAsync(InterviewSchedule schedule)=>_db.InterviewSchedules.AddAsync(schedule).AsTask();
    public void UpdateSchedule(InterviewSchedule schedule)=>_db.InterviewSchedules.Update(schedule);
    public Task SaveChangesAsync()=>_db.SaveChangesAsync();
}
