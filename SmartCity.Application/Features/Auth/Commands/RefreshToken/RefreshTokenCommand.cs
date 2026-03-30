using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Auth.DTOs;

namespace SmartCity.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<ApiResponse<LoginResponseDto>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}