using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Crypto
{
    public record KeyPair
    {
        public byte[] PrivateKey { get; init; }
        public byte[] PublicKey { get; init; }
    }
}
