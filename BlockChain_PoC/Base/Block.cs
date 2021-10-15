using BlockChain_PoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Base
{
    internal record Block : IValidatable
    {
        const int Difficulty = 2;
        public IEnumerable<Transaction> Transactions { get; init; }
        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
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
        public static Block CreateBlock(IEnumerable<Transaction> transactions, byte[] previousHash = null, DateTime? dateTime = null)
        {
            var block = new Block(transactions, previousHash, dateTime);

            return block;
        }
        public Block(IEnumerable<Transaction> transactions, byte[] previousHash = null, DateTime? dateTime = null)
        {
            this.Transactions = transactions?.ToList() ?? new List<Transaction>();
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

                    if (Transactions != null)
                    {
                        var transactionsBytes = MessagePack.MessagePackSerializer.Serialize(this.Transactions.ToArray());

                        ms.Write(transactionsBytes);
                    }
                    ms.Write(timeStampBytes);
                    ms.Write(this.Nonce);
                    if (this.PreviousHash != null)
                    {
                        ms.Write(this.PreviousHash);
                    }
                    var msArray = ms.ToArray();
                    var hashedBlock = hashFunction.ComputeHash(msArray);
                    return hashedBlock;
                }
            }
        }

        public bool IsValid()
        {
            foreach(var transaction in this.Transactions)
            {
                if (!transaction.IsValid()) return false;
            }
            return true;
        }
    }
}
