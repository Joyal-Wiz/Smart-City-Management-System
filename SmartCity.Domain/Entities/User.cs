using System;
using System.Collections.Generic;
using System.Text;
using SmartCity.Domain.Enums;

namespace SmartCity.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string PasswordHash { get; set; }   

        public UserRole Role { get; set; } = UserRole.Citizen;

        public bool IsBlocked { get; set; } = false;
    }
}
