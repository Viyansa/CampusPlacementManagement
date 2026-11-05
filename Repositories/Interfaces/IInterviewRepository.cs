using CampusPlacementManagement.Models;
namespace CampusPlacementManagement.Repositories.Interfaces;
public interface IInterviewRepository
{
    Task<InterviewRound?> GetRoundByIdAsync(int id);
    Task<List<InterviewRound>> GetRoundsByJobAsync(int jobId);
    Task<InterviewSchedule?> GetScheduleByIdAsync(int id);
    Task<List<InterviewSchedule>> GetSchedulesByApplicationAsync(int applicationId);
    Task AddRoundAsync(InterviewRound round);
    Task AddScheduleAsync(InterviewSchedule schedule);
    void UpdateSchedule(InterviewSchedule schedule);
    Task SaveChangesAsync();
}
