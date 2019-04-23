using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Fullbar.Olympus.Dominio.Util
{
    public static class Crypto
    {
        public static string Key256 = "dzllNWZLY0lzbHdzK0J4b0x2RXJLZz09LDNSM2JsL0FXaFBLMXRpaUNyNWZCczZCTCtPbXF3V1dVNVFaeTBsVTlLU0E9";

        public static string Encrypt(string plainStr, string completeEncodedKey, int keySize)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = keySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.IV = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(completeEncodedKey)).Split(',')[0]);
            aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(completeEncodedKey)).Split(',')[1]);
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(plainStr);
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();

            // The result of the encryption and decryption            
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherText);
        }

        public static string Decrypt(string encryptedText, string completeEncodedKey, int keySize)
        {

            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = keySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.IV = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(completeEncodedKey)).Split(',')[0]);
            aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(completeEncodedKey)).Split(',')[1]);
            ICryptoTransform decrypto = aesEncryption.CreateDecryptor();

            byte[] encryptedBytes = Convert.FromBase64CharArray(encryptedText.ToCharArray(), 0, encryptedText.Length);

            return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
        }

    }
}
