using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Repositories;
public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _db;
    public CompanyRepository(ApplicationDbContext db)=>_db=db;
    public Task<List<Company>> GetAllAsync()=>_db.Companies.Include(x=>x.CompanyHRs).AsNoTracking().OrderBy(x=>x.CompanyName).ToListAsync();
    public Task<List<Company>> GetPendingAsync()=>_db.Companies.Where(x=>x.Status==Models.Enums.CompanyStatus.Pending).AsNoTracking().ToListAsync();
    public Task<Company?> GetByIdAsync(int id)=>_db.Companies.Include(x=>x.CompanyHRs).FirstOrDefaultAsync(x=>x.CompanyId==id);
    public Task AddAsync(Company company)=>_db.Companies.AddAsync(company).AsTask();
    public void Update(Company company)=>_db.Companies.Update(company);
    public Task SaveChangesAsync()=>_db.SaveChangesAsync();
}
