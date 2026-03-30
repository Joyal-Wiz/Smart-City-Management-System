using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Auth.Commands.DTOs;

namespace SmartCity.Application.Features.Auth.Commands.Register
{
    public class RegisterUserCommand : IRequest<ApiResponse<RegisterResponseDto>> 
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public bool IsWorker { get; set; }

        public string Password { get; set; } = string.Empty;
    }
}