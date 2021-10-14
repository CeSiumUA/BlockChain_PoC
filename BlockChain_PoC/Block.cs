using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC
{
    internal record Block
    {
        public Guid Index = Guid.NewGuid();
        public byte[] Data { get; init; }
        public DateTime CreatedOn { get; init; } = DateTime.Now;
        public byte[] Hash
        {
            get
            {
                if (hash == null) CreateHash();
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
                CreateHash();
            }
        }
        public static Block CreateBlock<T>(T data, byte[] previousHash = null)
        {
            var serializedBytes = MessagePack.MessagePackSerializer.Serialize<T>(data);
            var block = new Block
            {
                Data = serializedBytes,
                PreviousHash = previousHash
            };
            return block;
        }
        private byte[] hash { get; set; }
        private byte[] previuosHash { get; set; }
        public void CreateHash()
        {
            var hashFunction = SHA256.Create();
            using (hashFunction)
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    var timeStampBytes = BitConverter.GetBytes(this.CreatedOn.ToBinary());
                    var guidBytes = this.Index.ToByteArray();

                    ms.Write(timeStampBytes);
                    ms.Write(guidBytes);
                    if (this.PreviousHash != null)
                    {
                        ms.Write(this.PreviousHash);
                    }
                    ms.Write(this.Data);
                    var hashedBlock = hashFunction.ComputeHash(ms);
                    hash = hashedBlock;
                }
            }
        }
    }
}
