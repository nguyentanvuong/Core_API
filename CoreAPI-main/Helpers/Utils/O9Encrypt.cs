using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Helpers.Utils
{
    public class O9Encrypt
    {
        private static string CONSTKEY = "abhf@311";
        public static string Decrypt (string textToDecrypt)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            Byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            Byte[] pwdBytes = Encoding.UTF8.GetBytes(CONSTKEY);
            Byte[] keyBytes = new Byte[16];
            int len = pwdBytes.Length;
            len = (len > keyBytes.Length) ? keyBytes.Length : pwdBytes.Length;
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            Byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }

        public static string Encrypt(string textToEncrypt)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 0x80;
            rijndaelCipher.BlockSize = 0x80;
            Byte[] pwdBytes = Encoding.UTF8.GetBytes(CONSTKEY);
            Byte[] keyBytes = new Byte[16];
            int len = pwdBytes.Length;
            len = (len > keyBytes.Length) ? keyBytes.Length : pwdBytes.Length;
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;

            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            Byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }

        public static string MD5Encrypt(string pwd)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            Byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            return Convert.ToBase64String(result);
        }

        public static string GenerateRandomString()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return null;
            }
        }

        public static string sha256(string password)
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = string.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            byte[] array = crypto;
            for (int i = 0; i < array.Length; i++)
            {
                byte bit = array[i];
                hash += bit.ToString("x2");
            }
            return hash;
        }

        public static string sha_sha256(string password, string loginName)
        {
            loginName = loginName.ToUpper();
            string satl = string.Empty;
            string outEnc = string.Empty;
            string shapassword = string.Empty;
            shapassword = O9Encrypt.sha256(password);
            if (shapassword.Length > 9)
            {
                satl = shapassword.Substring(6, 9).ToLower();
            }
            return O9Encrypt.sha256(shapassword + satl + loginName);
        }
    }
}