using System.Security.Claims;
using CampusPlacementManagement.DTOs.Auth;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace CampusPlacementManagement.Controllers;
[ApiController,Route("api/auth")]
public class AuthController:ControllerBase
{
 private readonly IAuthService _service;public AuthController(IAuthService service)=>_service=service;
 [AllowAnonymous,HttpPost("login")]public Task<TokenResponseDto> Login(LoginRequestDto dto)=>_service.LoginAsync(dto);
 [Authorize,HttpPost("refresh-token")]public Task<TokenResponseDto> Refresh(RefreshTokenRequestDto dto)=>_service.RefreshTokenAsync(dto.RefreshToken);
 [Authorize,HttpPost("change-password")]public async Task<IActionResult> Change(ChangePasswordDto dto){await _service.ChangePasswordAsync(UserId(),dto);return Ok(new{message="Password changed successfully."});}
 [AllowAnonymous,HttpPost("forgot-password")]public async Task<IActionResult> Forgot(ForgotPasswordDto dto){var token=await _service.CreatePasswordResetTokenAsync(dto);return Ok(new{message="If the account exists, password reset instructions have been generated.",developmentToken=string.IsNullOrEmpty(token)?null:token});}
 [AllowAnonymous,HttpPost("reset-password")]public async Task<IActionResult> Reset(ResetPasswordDto dto){await _service.ResetPasswordAsync(dto);return Ok(new{message="Password reset successfully."});}
 [Authorize,HttpGet("me")]public Task<object> Me()=>_service.GetMeAsync(UserId());
 private string UserId()=>User.FindFirstValue(ClaimTypes.NameIdentifier)??throw new UnauthorizedAccessException();
}
