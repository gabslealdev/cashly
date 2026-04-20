using Cashly.Application.Identity.Interfaces.Security;
using Cashly.Domain.Identity.ValueObjects;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace Cashly.Infrastructure.Security
{

    public sealed class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 1;
        private const int MemorySize = 1024 * 2;
        private const int DegreeOfParallelism = 1;

        public string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = ComputeHash(password, salt);

            return string.Join('.',
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        public bool Verify(string password, PasswordHash passwordHash)
        {
            var parts = passwordHash.Value.Split('.');

            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] expectedHash = Convert.FromBase64String(parts[1]);

            byte[] actualHash = ComputeHash(password, salt);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        private static byte[] ComputeHash(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = salt,
                Iterations = Iterations,
                MemorySize = MemorySize,
                DegreeOfParallelism = DegreeOfParallelism
            };

            return argon2.GetBytes(HashSize);
        }
    }
}
