using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Repositories;
public class CollegeRepository : ICollegeRepository
{
    private readonly ApplicationDbContext _db;
    public CollegeRepository(ApplicationDbContext db) => _db=db;
    public Task<List<College>> GetAllAsync()=>_db.Colleges.AsNoTracking().OrderBy(x=>x.CollegeName).ToListAsync();
    public Task<List<College>> GetPendingAsync()=>_db.Colleges.AsNoTracking().Where(x=>x.Status==Models.Enums.CollegeStatus.Pending).ToListAsync();
    public Task<College?> GetByIdAsync(int id)=>_db.Colleges.Include(x=>x.PlacementOfficers).FirstOrDefaultAsync(x=>x.CollegeId==id);
    public Task<College?> GetByCodeAsync(string code)=>_db.Colleges.FirstOrDefaultAsync(x=>x.CollegeCode==code);
    public Task AddAsync(College college)=>_db.Colleges.AddAsync(college).AsTask();
    public void Update(College college)=>_db.Colleges.Update(college);
    public Task SaveChangesAsync()=>_db.SaveChangesAsync();
}
