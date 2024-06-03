
using System.Security.Cryptography;
using System.Text;

namespace ISTA.SecureApp.Services
{
    public static class Encript
    {

        public static string EncriptPassword(string input)
        {
            SHA256 md5Hash = SHA256.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
