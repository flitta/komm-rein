using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.model
{

    public class Signed<T>
    {
        public T Payload { get; private set; }
        public string Signature { get; private set; }

        public Signed(T payload, string signature)
        {
            Payload = payload;
            Signature = signature;
        }
    }
}
