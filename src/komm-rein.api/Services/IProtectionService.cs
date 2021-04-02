using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public interface IProtectionService
    {
        string Encrypt<T>(T input);

        string Encrypt<T>(T input, TimeSpan? expiration);

        T Decrypt<T>(string input);

        string Sign<T>(T input);

        string Sign<T>(T input, TimeSpan? expiration);

        bool Verify<T>(string signature, T item);
    }
}
