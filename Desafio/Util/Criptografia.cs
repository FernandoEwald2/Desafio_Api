using System.Security.Cryptography;
using System.Text;
using System;

namespace Desafio.Util
{
    public class Criptografia
    {
        public static void CriarHashSalt(string str, out byte[] hash, out byte[] salt)
        {
            if (str == null)
                throw new Exception("");
            if (string.IsNullOrWhiteSpace(str))
                throw new Exception("");

            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(str));
            }
        }

        public static bool VerificarHashSalt(string str, byte[] hash, byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(str)) throw new Exception("Informe uma Senha");
            if (hash == null) throw new Exception("Não há uma senha cadastrada.");
            if (hash.Length != 64) throw new Exception("Invalid length of password hash (64 bytes expected).");
            if (salt.Length != 128) throw new Exception("Invalid length of password salt (128 bytes expected).");

            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(str));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hash[i]) return false;
                }
            }

            return true;
        }        
    }
}
