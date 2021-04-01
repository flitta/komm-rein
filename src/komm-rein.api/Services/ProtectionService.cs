﻿using Microsoft.AspNetCore.DataProtection;
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

        // the 'provider' parameter is provided by DI
        public ProtectionService(IDataProtectionProvider dataprotectionProvider)
        {
            _protector = dataprotectionProvider.CreateProtector("komm-rein.api")
                .ToTimeLimitedDataProtector();
           _sha512 = new SHA512Managed();
        }

        public string Encrypt<T>(T input, TimeSpan expiration)
        {
            string jsonString = JsonSerializer.Serialize(input);
            string protectedPayload = _protector.Protect(jsonString, expiration);

            return protectedPayload;
        }

        public T Decrypt<T>(string input)
        {
            string decryptedJson = _protector.Unprotect(input);
            T result = JsonSerializer.Deserialize<T>(decryptedJson);

            return result;
        }
        public string Sign<T>(T input, TimeSpan expiration)
        {
            string jsonString = JsonSerializer.Serialize(input);
            byte[] jsonBytes = Encoding.Default.GetBytes(jsonString);

            byte[] hash = _sha512.ComputeHash(jsonBytes);

            byte[] protectedPayload = _protector.Protect(hash, expiration);

            return Convert.ToBase64String(protectedPayload);
        }

        public bool Verify<T>(string signature, T item)
        {
            byte[] decryptedBytes = Convert.FromBase64String(signature);
            byte[] hashFromSignature = _protector.Unprotect(decryptedBytes);
            
          
            string jsonString = JsonSerializer.Serialize(item);
            byte[] jsonBytes = Encoding.Default.GetBytes(jsonString);
            byte[] hashFromInput = _sha512.ComputeHash(jsonBytes);

            bool result = Equality(hashFromSignature, hashFromInput);
            return result;
        }


        private bool Equality(byte[] a1, byte[] b1)
        {
            int i;
            if (a1.Length == b1.Length)
            {
                i = 0;
                while (i < a1.Length && (a1[i] == b1[i])) //Earlier it was a1[i]!=b1[i]
                {
                    i++;
                }
                if (i == a1.Length)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
