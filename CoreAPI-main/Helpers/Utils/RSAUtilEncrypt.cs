using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.Text;
using Org.BouncyCastle.Security;

namespace WebApi.Helpers.Utils
{
    public class RSAUtilEncrypt
    {
        /// <summary>
        /// Get RSA Key from path of pem file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static RsaKeyParameters GetRSAKey(string path)
        {
            var pemReader = new PemReader(File.OpenText(path));
            var rsaKey = (RsaKeyParameters)pemReader.ReadObject();
            return rsaKey;
        }

        /// <summary>
        /// Encrypt with public RSA Key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static string Encrypt(string data, RsaKeyParameters publicKey)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(data);
            var encryptEngine = CipherUtilities.GetCipher("RSA/ECB/OAEPWITHSHA_256ANDMGF1WITHSHA_1PADDING");

            if (publicKey == null)
            {
                throw new Exception("Invalid key!");
            }

            if (publicKey.IsPrivate)
            {
                throw new Exception("This key is not a public key!");
            }

            encryptEngine.Init(true, publicKey);

            if (bytesToEncrypt.Length > encryptEngine.GetBlockSize())
            {
                throw new Exception("Data length exceeds the encryption length of the key");
            }
            try
            {
                var encrypted = Convert.ToBase64String(encryptEngine.DoFinal(bytesToEncrypt));
                return encrypted;
            }
            catch (Exception e)
            {
                throw new Exception("Encryption failed because of " + e.Message);
            }
        }

        /// <summary>
        /// Encrypt with path of pem file
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pathPublicKey"></param>
        /// <returns></returns>
        public static string Encrypt(string data, string pathPublicKey)
        {
            var publicKey = GetRSAKey(pathPublicKey);
            return Encrypt(data, publicKey);
        }

        /// <summary>
        /// Decrypt with priavte RSA Key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string Decrypt(string data, RsaKeyParameters privateKey)
        {
            var bytesToDecrypt = Convert.FromBase64String(data);
            var decryptEngine = CipherUtilities.GetCipher("RSA/ECB/OAEPWITHSHA_256ANDMGF1WITHSHA_1PADDING");

            if (privateKey == null)
            {
                throw new Exception("Invalid key!");
            }

            if (!privateKey.IsPrivate)
            {
                throw new Exception("This key is not a private key!");
            }

            decryptEngine.Init(false, privateKey);

            if (bytesToDecrypt.Length > decryptEngine.GetBlockSize())
            {
                throw new FormatException("Data length exceeds the decryption length of the key");
            }
            try
            {
                var decrypted = Encoding.UTF8.GetString(decryptEngine.DoFinal(bytesToDecrypt));
                return decrypted;
            }
            catch (Exception e)
            {
                throw new Exception("Decryption failed because of " + e.Message);
            }
        }

        /// <summary>
        /// Decrypt with path of pem file
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pathPrivateKey"></param>
        /// <returns></returns>
        public static string Decrypt(string data, string pathPrivateKey)
        {
            var privateKey = GetRSAKey(pathPrivateKey);
            return Decrypt(data, privateKey);
        }

        /// <summary>
        /// Sign data with private RSA Key
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signPrivateKey"></param>
        /// <returns></returns>
        public static string SignData(string data, RsaKeyParameters signPrivateKey)
        {
            var bytesToSign = Encoding.UTF8.GetBytes(data);
            var signature = SignerUtilities.GetSigner("SHA256withRSA");

            if (signPrivateKey == null)
            {
                throw new Exception("Invalid key!");
            }

            if (!signPrivateKey.IsPrivate)
            {
                throw new Exception("This key is not a private key!");
            }

            signature.Init(true, signPrivateKey);

            try
            {
                signature.BlockUpdate(bytesToSign, 0, bytesToSign.Length);
                return Convert.ToBase64String(signature.GenerateSignature());
            }
            catch (Exception e)
            {
                throw new Exception("Sign data failed because of " + e.Message);
            }
        }

        /// <summary>
        /// Sign data with path of pem file
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pathSignPrivateKey"></param>
        /// <returns></returns>
        public static string SignData(string data, string pathSignPrivateKey)
        {
            var privateKey = GetRSAKey(pathSignPrivateKey);
            return SignData(data, privateKey);

        }

        /// <summary>
        /// Verify signature with plain text and public sign key
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="signature"></param>
        /// <param name="signPublicKey"></param>
        /// <returns></returns>
        public static bool Verify(string plainText, string signature, RsaKeyParameters signPublicKey)
        {
            var bytesPlainText = Encoding.UTF8.GetBytes(plainText);
            var publicSignature = SignerUtilities.GetSigner("SHA256withRSA");

            if (signPublicKey == null)
            {
                throw new Exception("Invalid key!");
            }

            if (signPublicKey.IsPrivate)
            {
                throw new Exception("This key is not a public key!");
            }

            publicSignature.Init(false, signPublicKey);

            try
            {
                publicSignature.BlockUpdate(bytesPlainText, 0, bytesPlainText.Length);
                var signatureBytes = Convert.FromBase64String(signature);
                return publicSignature.VerifySignature(signatureBytes);
            }
            catch (Exception e)
            {
                throw new Exception("Verify sign data failed because of " + e.Message);
            }
        }

        /// <summary>
        /// Verify signature with plain text and path of pem file
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="signature"></param>
        /// <param name="pathSignPublicKey"></param>
        /// <returns></returns>
        public static bool Verify(string plainText, string signature, string pathSignPublicKey)
        {
            var signPublicKey = GetRSAKey(pathSignPublicKey);
            return Verify(plainText, signature, signPublicKey);
        }

        /// <summary>
        /// Verify signature with cipher text, private decrypt key and public sign key
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="privateKey"></param>
        /// <param name="signature"></param>
        /// <param name="signPublicKey"></param>
        /// <returns></returns>
        public static bool Verify(string ciphertext, RsaKeyParameters privateKey, string signature, RsaKeyParameters signPublicKey)
        {
            var plainText = Decrypt(ciphertext, privateKey);
            return Verify(plainText, signature, signPublicKey);
        }

        /// <summary>
        /// Verify signature with cipher text, private decrypt key and path of the public signing key's pem file
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="privateKey"></param>
        /// <param name="signature"></param>
        /// <param name="pathSignPublicKey"></param>
        /// <returns></returns>
        public static bool Verify(string ciphertext, RsaKeyParameters privateKey, string signature, string pathSignPublicKey)
        {
            var signPublicKey = GetRSAKey(pathSignPublicKey);
            var plainText = Decrypt(ciphertext, privateKey);
            return Verify(plainText, signature, signPublicKey);
        }

        /// <summary>
        /// Verify signature with cipher text, path of the public decrypt key's pem file and public sign key 
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="pathPrivateKey"></param>
        /// <param name="signature"></param>
        /// <param name="signPublicKey"></param>
        /// <returns></returns>
        public static bool Verify(string ciphertext, string pathPrivateKey, string signature, RsaKeyParameters signPublicKey)
        {
            var privateKey = GetRSAKey(pathPrivateKey);
            var plainText = Decrypt(ciphertext, privateKey);
            return Verify(plainText, signature, signPublicKey);
        }

        /// <summary>
        /// Verify signature with cipher text, path of the public decrypt key's pem file and path of the public signing key's pem file
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <param name="pathPrivateKey"></param>
        /// <param name="signature"></param>
        /// <param name="pathSignPublicKey"></param>
        /// <returns></returns>
        public static bool Verify(string ciphertext, string pathPrivateKey, string signature, string pathSignPublicKey)
        {
            var privateKey = GetRSAKey(pathPrivateKey);
            var signPublicKey = GetRSAKey(pathSignPublicKey);
            var plainText = Decrypt(ciphertext, privateKey);
            return Verify(plainText, signature, signPublicKey);
        }
    }
}
