using MediatR;
using SmartCity.Application.Features.Auth.DTOs;

namespace SmartCity.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<LoginResponseDto>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}