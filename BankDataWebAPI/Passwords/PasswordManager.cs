using System.Security.Cryptography;

namespace BankDataWebAPI.Passwords
{
    public class PasswordManager
    {
        public static void CreatePasswordHash(
            string password,
            out string passwordHash,
            out string passwordSalt)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                passwordSalt = Convert.ToBase64String(salt);
                passwordHash = Convert.ToBase64String(hash);
            }
        }
        public static bool VerifyPassword(
            string password,
            string storedHash,
            string storedSalt)
        {
            byte[] salt = Convert.FromBase64String(storedSalt);
            byte[] expectedHash = Convert.FromBase64String(storedHash);

            using(var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                100000,
                HashAlgorithmName.SHA256))
            {
                byte[] actualHash = pbkdf2.GetBytes(32);

                return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
            }
        }
    }
}
