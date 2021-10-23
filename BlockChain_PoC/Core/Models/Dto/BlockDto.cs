using BlockChain_PoC.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.Models.Dto
{
    public record BlockDto : BaseDto
    {
        public int Difficulty { get; init; }
        public long Id { get; init; }
        public DateTime TimeStamp { get;init; } = DateTime.Now;
        public bool IsMined { get; init; }
        public byte[] Nonce { get; init; }
        public byte[] Hash { get; init; }
        public byte[] PreviousHash { get; init; }
        public List<TransactionDto> Transactions { get; init; } = new List<TransactionDto>();
        public Block ConvertToBlock()
        {
            var block = new Block(Transactions.Select(x => x.ConvertToTransaction()), Id, PreviousHash, Difficulty)
            {
                TimeStamp = TimeStamp,
                Hash = Hash,
                IsMined = IsMined,
                Nonce = Nonce,
            };
            return block;
        }
        public static BlockDto ConvertToDto(Block block)
        {
            return new BlockDto()
            {
                Difficulty = block.Difficulty,
                Id = block.Id,
                TimeStamp = block.TimeStamp,
                IsMined = block.IsMined,
                Nonce = block.Nonce,
                Hash = block.Hash,
                PreviousHash = block.PreviousHash,
                Transactions = block.Transactions.Select(x => TransactionDto.ConvertFromTransaction(x)).ToList(),
            };
        }
    }
}
