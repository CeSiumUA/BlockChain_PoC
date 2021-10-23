using BlockChain_PoC.Base;
using BlockChain_PoC.Interfaces;

namespace BlockChain_PoC.Core.Models.Dto
{
    public record TransactionDto
    {
        public Guid Id { get; init; }

        public byte[] From { get; init; } = new byte[0];

        public byte[] To { get; init; } = new byte[0];

        public byte[] Hash { get; init; } = new byte[0];

        public byte[] Signature { get; init; } = new byte[0];

        public byte[] MessageContent { get; init; } = new byte[0];

        public TransactionType TransactionType { get; init; }
        public ITransaction ConvertToTransaction()
        {
            switch (TransactionType)
            {
                case TransactionType.Message:
                    return new MessageTransaction(From, To, MessageContent)
                    {
                        Id = Id,
                        Hash = Hash,
                        Signature = Signature,
                    };
                default:
                    return new MessageTransaction(From, To, MessageContent)
                    {
                        Id = Id,
                        Hash = Hash,
                        Signature = Signature,
                    };
            }
        }
        public static TransactionDto ConvertFromTransaction(ITransaction transaction)
        {
            return new TransactionDto()
            {
                Id = transaction.Id,
                From = transaction.From,
                To = transaction.To,
                Hash = transaction.Hash,
                Signature = transaction.Signature,
                MessageContent = transaction.Content,
                TransactionType = transaction.TransactionType
            };
        }
    }
}
