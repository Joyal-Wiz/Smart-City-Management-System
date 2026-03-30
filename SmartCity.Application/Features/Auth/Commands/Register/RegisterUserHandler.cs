using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Auth.Commands.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ApiResponse<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponse<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLower();

            var existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                return ApiResponse<Guid>.FailResponse("Email already registered");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = email,
                PhoneNumber = request.PhoneNumber.Trim(),
                PasswordHash = _passwordHasher.Hash(request.Password),
                Role = UserRole.Citizen
            };

            await _userRepository.AddAsync(user);

            return ApiResponse<Guid>.SuccessResponse("User registered successfully", user.Id);
        }
    }
}