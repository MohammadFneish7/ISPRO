using System.Text;
using System.Security.Cryptography;

namespace ISPRO.Helpers
{
    public class CryptoHelper
    {
        public static string ComputeSHA256Hash(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            using (SHA256 sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }
    }
}