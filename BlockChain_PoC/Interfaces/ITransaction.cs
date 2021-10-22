using BlockChain_PoC.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Interfaces
{
    public interface ITransaction : IHashable
    {
        public Guid Id { get; init; }
        public byte[] From { get; init; }
        public byte[] To { get; init; }
        public string FromAddress { get; }
        public string ToAddress { get; }
        public byte[] Hash { get; }
        public byte[] Signature { get; }
        public void SignTransaction(KeyPair keyPair);
    }
}
