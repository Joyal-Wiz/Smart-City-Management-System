using Microsoft.AspNetCore.Http;
using SmartCity.Application.Interfaces;
using System;
using System.Security.Claims;

namespace SmartCity.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // ✅ GET USER ID FROM JWT
        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                return userId != null ? Guid.Parse(userId) : Guid.Empty;
            }
        }

        // 🔥 GET ROLE FROM JWT (VERY IMPORTANT)
        public string Role
        {
            get
            {
                return _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.Role)?
                    .Value ?? string.Empty;
            }
        }
    }
}