using System;
using System.Text;

using PCLCrypto;

namespace Xamarin.Auth.Cryptography
{
    public static class CryptoUtilities
    {
        const int IVSize = 16;

        public static byte[] GetAES256KeyMaterial()
        {
            return WinRTCrypto.CryptographicBuffer.GenerateRandom(32);
        }

        public static byte[] Get256BitSalt()
        {
            return WinRTCrypto.CryptographicBuffer.GenerateRandom(32);
        }

        public static byte[] Encrypt(byte[] plainText, byte[] keyMaterial)
        {
            //TODO
            return plainText;
        }

        public static byte[] Decrypt(byte[] cipherText, byte[] keyMaterial)
        {
            //TODO
            return cipherText;
        }

        public static byte[] GetHash(byte[] data, byte[] salt)
        {
            byte[] saltedData = new byte[data.Length + salt.Length];
            Array.Copy(salt, saltedData, salt.Length);
            Array.Copy(data, 0, saltedData, salt.Length, data.Length);

            var sha = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
            return sha.HashData(saltedData);
        }

        public static string ByteArrayToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public static byte[] StringToByteArray(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
    }
}