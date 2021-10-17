using BlockChain_PoC.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Base
{
    public record Block : IHashable
    {
        public int Difficulty { get; private set; }
        public long Id { get; init;}
        public DateTime TimeStamp { get; init; } = DateTime.Now;
        public List<ITransaction> Transactions { get; init; } = new List<ITransaction>();
        public bool IsMined
        {
            get
            {
                return isMinded;
            }
        }
        public byte[] Nonce
        {
            get
            {
                return nonce;
            }
        }
        public Block(IEnumerable<ITransaction> transactions, long id, byte[] previousHash, int difficulty = 3)
        {
            this.Transactions = transactions.ToList();
            this.Id = id;
            this.PreviousHash = previousHash;
            this.Difficulty = difficulty;
        }
        public void MineBlock()
        {
            using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                long tries = 0;
                do
                {
                    var nonceBytes = new byte[64];
                    rng.GetBytes(nonceBytes);
                    this.nonce = nonceBytes;
                } while (!IsDifficultyProofed());
            }
            isMinded = true;
        }
        public byte[] PreviousHash { get; init; }
        private byte[] nonce { get; set; }
        private bool isMinded { get; set; } = false;
        public byte[] GetHash()
        {
            using (var sha256 = SHA256.Create())
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    ms.Write(Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(this.Transactions)));
                    ms.Write(this.Nonce);
                    ms.Write(BitConverter.GetBytes(this.Difficulty));
                    ms.Write(BitConverter.GetBytes(this.Id));
                    ms.Write(this.PreviousHash);
                    ms.Write(BitConverter.GetBytes(this.TimeStamp.ToBinary()));
                    var bytesToHash = ms.ToArray();
                    return sha256.ComputeHash(bytesToHash);
                }
            }
        }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
        private bool IsDifficultyProofed()
        {
            var bits = new BitArray(GetHash());
            for(int x = 0; x < Difficulty; x++)
            {
                if(!bits[x]) return false;
            }
            return true;
        }
    }
}
