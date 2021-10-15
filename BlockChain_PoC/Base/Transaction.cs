using BlockChain_PoC.Crypto;
using BlockChain_PoC.Interfaces;
using MessagePack;
using Secp256k1Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Base
{
    [MessagePackObject]
    public record Transaction : IValidatable
    {
        [Key(0)]
        public string From { get; init; } = string.Empty;
        [Key(1)]
        public string To { get; init; } = string.Empty;
        [Key(2)]
        public long Amount { get; init; }
        [Key(3)]
        public byte[] Hash { get; private set; }
        [Key(4)]
        public byte[] Signature
        {
            get
            {
                return signature;
            }
        }
        public Transaction()
        {
            CalculateHash();
            this.Hash = CalculateHash();
        }
        public byte[] CalculateHash()
        {
            using (var sha256 = SHA256.Create())
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    ms.Write(BitConverter.GetBytes(Amount));
                    ms.Write(Encoding.UTF8.GetBytes(From));
                    ms.Write(Encoding.UTF8.GetBytes(To));
                    var bytes = ms.ToArray();
                    return sha256.ComputeHash(bytes);
                }
            }
        }

        public void SignTransaction(KeyPair keyPair)
        {
            if(keyPair.PublicKey.GetHex() != this.From)
            {
                throw new InvalidOperationException("You have no permission for this operation!");
            }
            var hash = CalculateHash();
            using(var secp256k1 = new Secp256k1())
            {
                var signature = new byte[64];
                secp256k1.Sign(signature, hash, keyPair.PrivateKey);
                this.signature = signature;
            }
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(From)) return true;

            if(signature == null || signature.Length == 0)
            {
                throw new Exception("Transaction is not signed!");
            }

            using (var secp256k1 = new Secp256k1())
            {
                var hexBytes = From.ToHexBytes();
                var validity = secp256k1.Verify(signature, CalculateHash(), hexBytes);
                return validity;
            }
        }

        private byte[] signature { get; set; }
    }
}
