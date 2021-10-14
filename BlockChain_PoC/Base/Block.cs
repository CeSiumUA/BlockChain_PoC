using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Base
{
    internal record Block
    {
        const int Difficulty = 3;
        public byte[] Data { get; init; }
        public DateTime CreatedOn { get; init; } = DateTime.Now;
        public byte[] Nonce
        {
            get
            {
                return nonce;
            }
            private set
            {
                nonce = value;
                hash = CreateHash();
            }
        }
        public byte[] Hash
        {
            get
            {
                if (hash == null)
                {
                    hash = CreateHash();
                }
                return hash;
            }
        }
        public byte[] PreviousHash
        {
            get
            {
                return previuosHash;
            }
            set
            {
                previuosHash = value;
                hash = CreateHash();
            }
        }
        public static Block CreateBlock<T>(T data, byte[] previousHash = null, DateTime? dateTime = null)
        {
            var serializedBytes = MessagePack.MessagePackSerializer.Serialize<T>(data);

            var block = new Block(serializedBytes, previousHash, dateTime);

            return block;
        }
        public Block(byte[] data, byte[] previousHash = null, DateTime? dateTime = null)
        {
            this.Data = data;
            this.PreviousHash = previousHash;
            if(dateTime.HasValue) this.CreatedOn = dateTime.Value;
            MineBlock();
        }
        private void MineBlock()
        {
            using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                long tries = 0;
                do
                {
                    var nonceBytes = new byte[8];
                    rng.GetBytes(nonceBytes);

                    this.Nonce = nonceBytes;
                    tries++;
                }
                while (!DifficultyProofed());
            }
        }
        private bool DifficultyProofed()
        {
            if(!Enumerable.SequenceEqual<byte>(this.Nonce.Take(Difficulty), Enumerable.Repeat<byte>(0, Difficulty)))
            {
                return false;
            }
            return true;
        }
        private byte[] hash { get; set; }
        private byte[] nonce { get; set; }
        private byte[] previuosHash { get; set; }
        public byte[] CreateHash()
        {
            var hashFunction = SHA256.Create();
            using (hashFunction)
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    var timeStampBytes = BitConverter.GetBytes(this.CreatedOn.ToBinary());

                    ms.Write(timeStampBytes);
                    ms.Write(this.Nonce);
                    if (this.PreviousHash != null)
                    {
                        ms.Write(this.PreviousHash);
                    }
                    ms.Write(this.Data);
                    var msArray = ms.ToArray();
                    var hashedBlock = hashFunction.ComputeHash(msArray);
                    return hashedBlock;
                }
            }
        }
    }
}
