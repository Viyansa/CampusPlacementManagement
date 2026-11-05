using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    public AuthService(UserManager<ApplicationUser> userManager,IConfiguration configuration,IWebHostEnvironment environment)
    { _userManager=userManager; _configuration=configuration; _environment=environment; }

    public async Task<TokenResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user=await _userManager.FindByEmailAsync(dto.Email);
        if(user is null || user.AccountStatus!=AccountStatus.Approved || !user.EmailConfirmed) throw new UnauthorizedAccessException("Invalid credentials or inactive account.");
        if(!await _userManager.CheckPasswordAsync(user,dto.Password)) throw new UnauthorizedAccessException("Invalid credentials or inactive account.");
        return await IssueTokensAsync(user);
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var hash=HashToken(refreshToken);
        var user=await _userManager.Users.FirstOrDefaultAsync(x=>x.RefreshTokenHash==hash);
        if(user is null || user.RefreshTokenExpiresAt<=DateTime.UtcNow || user.RefreshTokenRevokedAt!=null || user.AccountStatus!=AccountStatus.Approved) throw new UnauthorizedAccessException("Invalid refresh token.");
        user.RefreshTokenRevokedAt=DateTime.UtcNow; await _userManager.UpdateAsync(user);
        return await IssueTokensAsync(user);
    }

    public async Task ChangePasswordAsync(string userId,ChangePasswordDto dto)
    {
        var user=await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found.");
        var result=await _userManager.ChangePasswordAsync(user,dto.CurrentPassword,dto.NewPassword);
        if(!result.Succeeded) throw new InvalidOperationException(string.Join("; ",result.Errors.Select(x=>x.Description)));
    }

    public async Task<string> CreatePasswordResetTokenAsync(ForgotPasswordDto dto)
    {
        var user=await _userManager.FindByEmailAsync(dto.Email);
        if(user is null) return string.Empty;
        var token=await _userManager.GeneratePasswordResetTokenAsync(user);
        return _environment.IsDevelopment()?token:string.Empty;
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user=await _userManager.FindByEmailAsync(dto.Email) ?? throw new InvalidOperationException("Unable to reset password.");
        var result=await _userManager.ResetPasswordAsync(user,dto.Token,dto.NewPassword);
        if(!result.Succeeded) throw new InvalidOperationException(string.Join("; ",result.Errors.Select(x=>x.Description)));
    }

    public async Task<object> GetMeAsync(string userId)
    {
        var user=await _userManager.FindByIdAsync(userId) ?? throw new KeyNotFoundException("User not found.");
        return new { user.Id,user.FullName,user.Email,user.PhoneNumber,user.AccountStatus,Roles=await _userManager.GetRolesAsync(user) };
    }

    private async Task<TokenResponseDto> IssueTokensAsync(ApplicationUser user)
    {
        var roles=await _userManager.GetRolesAsync(user);
        var expires=DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:AccessTokenMinutes",30));
        var claims=new List<Claim>{new(JwtRegisteredClaimNames.Sub,user.Id),new(JwtRegisteredClaimNames.Email,user.Email??""),new(ClaimTypes.NameIdentifier,user.Id),new(ClaimTypes.Name,user.FullName),new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())};
        claims.AddRange(roles.Select(r=>new Claim(ClaimTypes.Role,r)));
        var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
        var token=new JwtSecurityToken(_configuration["Jwt:Issuer"],_configuration["Jwt:Audience"],claims,expires:expires,signingCredentials:creds);
        var access=new JwtSecurityTokenHandler().WriteToken(token);
        var refresh=Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshTokenHash=HashToken(refresh); user.RefreshTokenExpiresAt=DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshTokenDays",7)); user.RefreshTokenRevokedAt=null; user.UpdatedAt=DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return new TokenResponseDto{AccessToken=access,RefreshToken=refresh,AccessTokenExpiresAt=expires,RefreshTokenExpiresAt=user.RefreshTokenExpiresAt.Value,UserId=user.Id,Email=user.Email??"",FullName=user.FullName,Roles=roles.ToArray()};
    }
    private static string HashToken(string token)
    {
        using var sha=SHA256.Create(); return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(token)));
    }
}
