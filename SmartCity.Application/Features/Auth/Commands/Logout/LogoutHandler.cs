using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Interfaces;

namespace SmartCity.Application.Features.Auth.Commands.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public LogoutHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            //  Find refresh token in DB
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == request.RefreshToken, cancellationToken);

            //  If not found
            if (token == null)
                return false;

            //  Remove token
            _context.RefreshTokens.Remove(token);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}