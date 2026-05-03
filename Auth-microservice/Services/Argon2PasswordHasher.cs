using Auth_microservice.Domain.Interfaces;
using Isopoh.Cryptography.Argon2;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Auth_microservice.Services
{
    public class Argon2PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;

        private const int TimeCost = 4;
        private const int MemoryCost = 65536;
        private const int Lanes = 2;

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);

            var config = new Argon2Config
            {
                Type = Argon2Type.HybridAddressing,
                Version = Argon2Version.Nineteen,
                TimeCost = TimeCost,
                MemoryCost = MemoryCost,
                Lanes = Lanes,
                Threads = Lanes,
                Salt = salt,
                Password = Encoding.UTF8.GetBytes(password),
                HashLength = HashSize
            };

            var argon2 = new Argon2(config);
            var result = argon2.Hash();

            var saltBase64 = Convert.ToBase64String(salt);
            var hashBase64 = Convert.ToBase64String(result.Buffer);

            return $"{saltBase64}.{hashBase64}";
        }

        public bool Verify(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
                return false;

            var parts = hashedPassword.Split('.');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            var config = new Argon2Config
            {
                Type = Argon2Type.HybridAddressing,
                Version = Argon2Version.Nineteen,
                TimeCost = TimeCost,
                MemoryCost = MemoryCost,
                Lanes = Lanes,
                Threads = Lanes,
                Salt = salt,
                Password = Encoding.UTF8.GetBytes(password),
                HashLength = HashSize
            };

            var argon2 = new Argon2(config);
            var result = argon2.Hash();

            return CryptographicOperations.FixedTimeEquals(
                result.Buffer,
                storedHash
            );
        }
    }
}