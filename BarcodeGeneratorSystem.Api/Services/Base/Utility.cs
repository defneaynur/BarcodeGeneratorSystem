using System.Security.Cryptography;
using System.Text;


namespace BarcodeGeneratorSystem.Api.Services.Base
{
    public static class Utility
    {
        /// <summary>
        /// Password Convert to format sha256
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                var builder = new StringBuilder();

                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Password verify method
        /// </summary>
        /// <param name="enteredPassword"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var hashedEnteredPassword = HashPassword(enteredPassword);

            return hashedEnteredPassword.Equals(storedHash);
        }
    }
}
