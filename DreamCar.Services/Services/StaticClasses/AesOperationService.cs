using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DreamCar.Services.Services
{
    public static class AesOperationService
    {
        private static IConfiguration _configuration;

        public static void Initiate(IConfiguration configuration) => _configuration = configuration;
        public static string GetEmailApiKey()
        {
            return Decrypt(
                GetKey(), 
                _configuration.GetSection(
                    AppSettingsService.SectionName
                 ).GetValue<string>("ApiKey")
           );
        }

        public static string GetReCaptchaSiteKey()
        {
            return _configuration.GetValue<string>("ReCaptchaSiteKey");
        }

        public static string GetReCaptchaSecretKey()
        {
            return _configuration.GetValue<string>("ReCaptchaSecretKey");
        }

        private static string GetKey()
        {
            return _configuration.GetValue<string>("Key");
        }
        private static string Encrypt(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using(Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }
        private static string Decrypt(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}
