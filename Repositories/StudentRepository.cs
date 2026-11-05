using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Repositories;
public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _db;
    public StudentRepository(ApplicationDbContext db)=>_db=db;
    public Task<List<Student>> GetAllAsync(int? collegeId=null,string? department=null,int? graduationYear=null,string? search=null)
    {
        var q=_db.Students.Include(x=>x.User).Include(x=>x.College).Include(x=>x.PlacementOfficer).AsQueryable();
        if(collegeId.HasValue) q=q.Where(x=>x.CollegeId==collegeId.Value);
        if(!string.IsNullOrWhiteSpace(department)) q=q.Where(x=>x.Department==department);
        if(graduationYear.HasValue) q=q.Where(x=>x.GraduationYear==graduationYear.Value);
        if(!string.IsNullOrWhiteSpace(search)) q=q.Where(x=>x.RegisterNumber.Contains(search)||x.User.FullName.Contains(search));
        return q.AsNoTracking().OrderBy(x=>x.RegisterNumber).ToListAsync();
    }
    public Task<List<Student>> GetPendingAsync(int collegeId)=>_db.Students.Include(x=>x.User).Where(x=>x.CollegeId==collegeId&&x.Status==Models.Enums.AccountStatus.Pending).AsNoTracking().ToListAsync();
    public Task<Student?> GetByIdAsync(int id)=>_db.Students.Include(x=>x.User).Include(x=>x.College).Include(x=>x.PlacementOfficer).FirstOrDefaultAsync(x=>x.StudentId==id);
    public Task<Student?> GetByUserIdAsync(string userId)=>_db.Students.Include(x=>x.College).Include(x=>x.PlacementOfficer).FirstOrDefaultAsync(x=>x.UserId==userId);
    public Task<bool> RegisterNumberExistsAsync(int collegeId,string registerNumber)=>_db.Students.AnyAsync(x=>x.CollegeId==collegeId&&x.RegisterNumber==registerNumber);
    public Task AddAsync(Student student)=>_db.Students.AddAsync(student).AsTask();
    public void Update(Student student)=>_db.Students.Update(student);
    public Task SaveChangesAsync()=>_db.SaveChangesAsync();
}
