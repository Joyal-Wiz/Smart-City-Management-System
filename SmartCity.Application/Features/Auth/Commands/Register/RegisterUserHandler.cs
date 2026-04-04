using MediatR;
using SmartCity.Application.DTOs;
using SmartCity.Application.Features.Auth.Commands.DTOs;
using SmartCity.Application.Interfaces;
using SmartCity.Domain.Entities;
using SmartCity.Domain.Enums;
using SmartCity.Domain.Interfaces;

namespace SmartCity.Application.Features.Auth.Commands.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ApiResponse<RegisterResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IWorkerRepository _workerRepository;
        private readonly INotificationService _notificationService; // 🔥 ADD

        public RegisterUserHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IWorkerRepository workerRepository,
            INotificationService notificationService) // 🔥 ADD
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _workerRepository = workerRepository;
            _notificationService = notificationService; // 🔥 ADD
        }

        public async Task<ApiResponse<RegisterResponseDto>> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim().ToLower();

            var existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                return ApiResponse<RegisterResponseDto>.FailResponse("Email already registered");
            }

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

            // 🔥 WORKER CREATION
            if (user.Role == UserRole.Worker)
            {
                var worker = new Worker
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Status = WorkerStatus.Pending,
                    IsAvailable = false
                };

                await _workerRepository.AddAsync(worker);

                // 🔥 ADD NOTIFICATION (IMPORTANT)
                await _notificationService.CreateAsync(
                    "New Worker Registered",
                    $"Worker {user.Name} has registered and is waiting for approval",
                    "Worker",
                    worker.Id
                );
            }

            return ApiResponse<RegisterResponseDto>.SuccessResponse(
                "User registered successfully",
                new RegisterResponseDto
                {
                    UserId = user.Id
                });
        }
    }
}