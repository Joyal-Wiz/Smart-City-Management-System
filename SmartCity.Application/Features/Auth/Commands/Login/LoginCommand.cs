using MediatR;
using SmartCity.Application.DTOs;

namespace SmartCity.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<ApiResponse<AuthResponseDto>>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}