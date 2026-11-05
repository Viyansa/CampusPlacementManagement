using CampusPlacementManagement.DTOs.Auth;
namespace CampusPlacementManagement.Services.Interfaces;
public interface IAuthService
{
    Task<TokenResponseDto> LoginAsync(LoginRequestDto dto);
    Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    Task ChangePasswordAsync(string userId, ChangePasswordDto dto);
    Task<string> CreatePasswordResetTokenAsync(ForgotPasswordDto dto);
    Task ResetPasswordAsync(ResetPasswordDto dto);
    Task<object> GetMeAsync(string userId);
}
