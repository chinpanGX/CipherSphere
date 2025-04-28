using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CipherSphere.Runtime.Security
{
    internal sealed class CryptographyExecutor
    {
        private static readonly int KeySizeInBytes = 16;
        private static readonly int IterationCount = 10000;
        private static string AppSalt = string.Empty; // アプリ側で自由に変更可能なSalt
        
        public static void Setup(string appSalt)
        {
            AppSalt = appSalt;
        }

        public static byte[] Encrypt(string plainText, string password)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            ValidateInput(plainTextBytes, password);
            var salt = GenerateSalt();
            using var aes = CreateAes();
            using var deriveBytes = new Rfc2898DeriveBytes(password, salt, IterationCount, HashAlgorithmName.SHA256);
            aes.Key = deriveBytes.GetBytes(aes.KeySize / 8);
            aes.IV = deriveBytes.GetBytes(aes.BlockSize / 8);
            using var encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
        }

        public static string Decrypt(byte[] cipherTextBytes, string password)
        {
            ValidateInput(cipherTextBytes, password);
            var salt = GenerateSalt();
            using var aes = CreateAes();
            using var deriveBytes = new Rfc2898DeriveBytes(password, salt, IterationCount, HashAlgorithmName.SHA256);
            aes.Key = deriveBytes.GetBytes(aes.KeySize / 8);
            aes.IV = deriveBytes.GetBytes(aes.BlockSize / 8);
            using var decryptor = aes.CreateDecryptor();
            var decrypted = decryptor.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);
            return Encoding.UTF8.GetString(decrypted);
        }

        private static byte[] GenerateSalt()
        {
            return SHA256.Create().ComputeHash(GetSalt());
        }

        private static void ValidateInput(byte[] inputBytes, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new DataStorageSecurityException("Password cannot be null or empty", nameof(password));
            if (inputBytes == null || inputBytes.Length == 0)
                throw new DataStorageSecurityException("Input bytes cannot be null or empty", nameof(inputBytes));
            if (password.Length < 8)
                throw new DataStorageSecurityException("Password must be at least 8 characters long", nameof(password)
                );
            if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
                throw new DataStorageSecurityException("Password must contain both letters and digits",
                    nameof(password)
                );
        }

        private static Aes CreateAes()
        {
            var aes = Aes.Create();
            aes.BlockSize = KeySizeInBytes * 8;
            aes.KeySize = KeySizeInBytes * 8;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }
        
        private static byte[] GetSalt()
        {
            if (string.IsNullOrEmpty(AppSalt))
            {
                var builtInSalt = "Zs0&)X#V2Mn!R8v+Lm$@3XpQa!Z5TYp9*oX3LtQa$V2Mn!R8v+L))hsm$@";   
                return Encoding.UTF8.GetBytes(builtInSalt); 
            }
            return Encoding.UTF8.GetBytes(AppSalt);
        }
    }
}