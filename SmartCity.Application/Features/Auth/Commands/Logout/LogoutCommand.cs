using MediatR;

namespace SmartCity.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommand : IRequest<bool>
    {
        public string RefreshToken { get; set; }
    }
}
