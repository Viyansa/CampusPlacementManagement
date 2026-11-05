using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Repositories;
public class PlacementOfficerRepository : IPlacementOfficerRepository
{
    private readonly ApplicationDbContext _db;
    public PlacementOfficerRepository(ApplicationDbContext db)=>_db=db;
    public Task<List<PlacementOfficer>> GetAllAsync()=>_db.PlacementOfficers.Include(x=>x.User).Include(x=>x.College).AsNoTracking().ToListAsync();
    public Task<List<PlacementOfficer>> GetPendingAsync()=>_db.PlacementOfficers.Include(x=>x.User).Include(x=>x.College).Where(x=>x.Status==Models.Enums.AccountStatus.Pending).AsNoTracking().ToListAsync();
    public Task<PlacementOfficer?> GetByIdAsync(int id)=>_db.PlacementOfficers.Include(x=>x.User).Include(x=>x.College).FirstOrDefaultAsync(x=>x.PlacementOfficerId==id);
    public Task<PlacementOfficer?> GetByUserIdAsync(string userId)=>_db.PlacementOfficers.Include(x=>x.College).FirstOrDefaultAsync(x=>x.UserId==userId);
    public Task AddAsync(PlacementOfficer officer)=>_db.PlacementOfficers.AddAsync(officer).AsTask();
    public void Update(PlacementOfficer officer)=>_db.PlacementOfficers.Update(officer);
    public Task SaveChangesAsync()=>_db.SaveChangesAsync();
}
