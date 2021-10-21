using BlockChain_PoC.Core;
using BlockChain_PoC.Crypto;
using BlockChain_PoC.Interfaces;
using Secp256k1Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Base
{
    public record MessageTransaction : ITransaction
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public byte[] From { get; init; } = new byte[0];
        public string FromAddress
        {
            get
            {
                return From.GetHex();
            }
        }
        public byte[] To { get; init; } = new byte[0];
        public string ToAddress
        {
            get
            {
                return To.GetHex();
            }
        }
        public byte[] MessageContent { get; init; }
        public byte[] Hash
        {
            get
            {
                return hash;
            }
        }
        public byte[] Signature
        {
            get
            {
                return signature;
            }
        }
        public MessageTransaction(string from, string to, byte[] message)
        {
            this.From = from.ToHexBytes();
            this.To = to.ToHexBytes();
            this.MessageContent = message;
        }
        public MessageTransaction(byte[] from, byte[] to, byte[] message)
        {
            this.From = from;
            this.To = to;
            this.MessageContent = message;
        }
        public bool IsValid()
        {
            if (signature == null || signature.Length == 0)
            {
                throw new Exception("Transaction is not signed!");
            }
            using (var secp256k1 = new Secp256k1())
            {
                var validity = secp256k1.Verify(signature, Hash, From);
                return validity;
            }
        }
        public void SignTransaction(KeyPair keyPair)
        {
            if (keyPair.PublicKey.GetHex() != this.FromAddress)
            {
                throw new InvalidOperationException("You have no permission for this operation!");
            }
            this.hash = GetHash();
            using(var secp256k1 = new Secp256k1())
            {
                secp256k1.Sign(signature, Hash, keyPair.PrivateKey);
            }
        }
        public byte[] GetHash()
        {
            using(var sha256 = SHA256.Create())
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    ms.Write(MessageContent);
                    ms.Write(From);
                    ms.Write(To);
                    ms.Write(Id.ToByteArray());
                    var bytes = ms.ToArray();
                    return sha256.ComputeHash(bytes);
                }
            }
        }
        private byte[] signature { get; set; } = new byte[64];
        private byte[] hash { get; set; }
    }
}
