using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{

    public class ProtectionService : IProtectionService
    {
        readonly ITimeLimitedDataProtector _protector;
        readonly SHA512Managed _sha512;

        public ProtectionService(IDataProtectionProvider dataprotectionProvider)
        {
            _protector = dataprotectionProvider.CreateProtector("komm-rein.api")
                .ToTimeLimitedDataProtector();
            _sha512 = new SHA512Managed();
        }

        public string Encrypt<T>(T input)
        {
            return Encrypt(input, null);
        }

        public string Encrypt<T>(T input, TimeSpan? expiration)
        {
            string jsonString = JsonSerializer.Serialize(input);
         
            string protectedPayload = expiration.HasValue
                ? _protector.Protect(jsonString, expiration.Value)
                : _protector.Protect(jsonString);

            return protectedPayload;
        }
                public T Decrypt<T>(string input)
        {
            string decryptedJson = _protector.Unprotect(input);
            T result = JsonSerializer.Deserialize<T>(decryptedJson);

            return result;
        }

        public string Sign<T>(T input)
        {
            return Sign(input, null);
        }
        
        public string Sign<T>(T input, TimeSpan? expiration)
        {
            string jsonString = JsonSerializer.Serialize(input);
            byte[] jsonBytes = Encoding.Default.GetBytes(jsonString);

            byte[] jsonHash = _sha512.ComputeHash(jsonBytes);

            byte[] jsonHashEncrypted = expiration.HasValue
                ? _protector.Protect(jsonHash, expiration.Value)
                : _protector.Protect(jsonHash);

            return Convert.ToBase64String(jsonHashEncrypted);
        }
           
        public bool Verify<T>(string signature, T item)
        {
            byte[] inputBytesEncrypted = Convert.FromBase64String(signature);
            byte[] inputBytes = _protector.Unprotect(inputBytesEncrypted);
                      
            string jsonString = JsonSerializer.Serialize(item);
            byte[] jsonBytes = Encoding.Default.GetBytes(jsonString);
            byte[] jsonHashBytes = _sha512.ComputeHash(jsonBytes);

            bool result = CompareByteArrays(inputBytes, jsonHashBytes);
            return result;
        }

        private bool CompareByteArrays(byte[] arr_1, byte[] arr_2)
        {
            if (arr_1.Length == arr_2.Length)
            {
                int index = 0;
                while (index < arr_1.Length && (arr_1[index] == arr_2[index]))
                {
                    index++;
                }
                if (index == arr_1.Length)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
