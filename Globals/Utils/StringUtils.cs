using System.Security.Cryptography;
using System.Text;

namespace NaughtyChoppersDA.Globals.Utils
{
    public static class StringUtils
    {
        public static string HashStringToSHA512(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte hashByte in hashBytes)
                {
                    stringBuilder.Append(hashByte.ToString("X2")); // X2 for uppercase hexadecimal format
                }

                return stringBuilder.ToString();
            }
        }
        public static bool ContainsUpperAndLowerCases(string text)
        {
            bool hasUppercase = text.Any(char.IsUpper);
            bool hasLowercase = text.Any(char.IsLower);

            return hasUppercase && hasLowercase;
        }
    }
}
