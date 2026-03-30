using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, ApiResponse<AuthResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public LoginHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLower();

            // 1. Get user
            var user = await _userRepository.GetByEmailAsync(email);

            // 2. Validate credentials (NO exception)
            if (user == null || !_passwordHasher.Verify(user.PasswordHash, request.Password))
            {
                return ApiResponse<AuthResponseDto>.FailResponse("Invalid email or password");
            }

            // 3. Generate token
            var token = _jwtService.GenerateToken(user);

            // 4. Return response
            var response = new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return ApiResponse<AuthResponseDto>.SuccessResponse("Login successful", response);
        }
    }
}
