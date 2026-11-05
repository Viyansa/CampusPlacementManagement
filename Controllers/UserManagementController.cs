using System.Security.Claims;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Controllers;
[ApiController,Route("api/users"),Authorize(Roles="SuperAdmin")]
public class UserManagementController:ControllerBase
{
 private readonly ApplicationDbContext _db;public UserManagementController(ApplicationDbContext db)=>_db=db;
 [HttpGet]public async Task<IActionResult> GetAll()=>Ok(await _db.Users.AsNoTracking().Select(x=>new{x.Id,x.FullName,x.Email,x.AccountStatus,x.CreatedAt,x.UpdatedAt}).ToListAsync());
 [HttpPut("{id}/status")]public async Task<IActionResult> UpdateStatus(string id,[FromQuery]AccountStatus status){var u=await _db.Users.FindAsync(id);if(u==null)return NotFound();u.AccountStatus=status;u.UpdatedAt=DateTime.UtcNow;await _db.SaveChangesAsync();return Ok(new{message="Status updated."});}
}
