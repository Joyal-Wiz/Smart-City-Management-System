using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartCity.Application.Features.Auth.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Application.Exceptions;

namespace SmartCity.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public RefreshTokenHandler(IApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var user = await _context.Users
                .FindAsync(new object[] { storedToken.UserId }, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var newAccessToken = _jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = request.RefreshToken,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}