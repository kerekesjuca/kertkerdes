using System.Security.Cryptography;
using System.Text;

namespace KertKerdes.Helpers
{
    public static class JelszoHelper
    {
        public static string HashJelszo(string jelszo)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(jelszo);
                var hash = sha256.ComputeHash(bytes);

                var builder = new StringBuilder();

                foreach (var b in hash)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}