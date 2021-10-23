using BlockChain_PoC.Base;
using BlockChain_PoC.Crypto;

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
        public byte[] Content { get; }
        public TransactionType TransactionType { get; }
        public void SignTransaction(KeyPair keyPair);
    }
}
