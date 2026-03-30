using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Auth.DTOs;

public class LoginCommand : IRequest<ApiResponse<LoginResponseDto>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}