using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
   using System.Security.Cryptography;
    using System.Text;
using SmartCity.Application.Interfaces;
namespace SmartCity.Infrastructure.Services
{
 

    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool Verify(string hash, string password)
        {
            var hashedPassword = Hash(password);
            return hashedPassword == hash;
        }
    }
}
