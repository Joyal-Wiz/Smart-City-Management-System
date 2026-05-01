using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Auth.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IApplicationDbContext _context;

        public LoginHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService,
            IApplicationDbContext context)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _context = context;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLower();

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !_passwordHasher.Verify(user.PasswordHash, request.Password))
            {
                throw new Exception("Invalid email or password");
            }

            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new SmartCity.Domain.Entities.RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Email = user.Email,
                Role = user.Role.ToString(),
                Name = user.Name
            };
        }
    }
}