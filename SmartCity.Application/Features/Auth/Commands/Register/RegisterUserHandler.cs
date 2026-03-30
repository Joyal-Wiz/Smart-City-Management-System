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
        private readonly IWorkerRepository _workerRepository;

        public RegisterUserHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IWorkerRepository workerRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _workerRepository = workerRepository;
        }

        public async Task<ApiResponse<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // 🔹 Normalize email
            var email = request.Email.Trim().ToLower();

            // 🔹 Check if user exists
            var existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                return ApiResponse<Guid>.FailResponse("Email already registered");
            }

            // 🔹 Create User
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = email,
                PhoneNumber = request.PhoneNumber.Trim(),
                PasswordHash = _passwordHasher.Hash(request.Password),
                Role = request.IsWorker ? UserRole.Worker : UserRole.Citizen
            };

            await _userRepository.AddAsync(user);

            // 🔥 Create Worker (if selected)
            if (user.Role == UserRole.Worker)
            {
                var worker = new Worker
                {
                    Id = Guid.NewGuid(),
                    Name = user.Name,
                    IsAvailable = false,
                    Status = "Pending"
                };

                await _workerRepository.AddAsync(worker);
            }

            return ApiResponse<Guid>.SuccessResponse("User registered successfully", user.Id);
        }
    }
}