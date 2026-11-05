using System.Security.Claims;using CampusPlacementManagement.Data;using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Middleware;
public class JwtRevocationMiddleware
{
 private readonly RequestDelegate _next;public JwtRevocationMiddleware(RequestDelegate next)=>_next=next;
 public async Task InvokeAsync(HttpContext context,ApplicationDbContext db){if(context.User.Identity?.IsAuthenticated==true){var id=context.User.FindFirstValue(ClaimTypes.NameIdentifier);if(!string.IsNullOrWhiteSpace(id)){var user=await db.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);if(user==null||user.AccountStatus!=Models.Enums.AccountStatus.Approved){context.Response.StatusCode=StatusCodes.Status401Unauthorized;return;}}}await _next(context);}
}
