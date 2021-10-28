using BlockChain_PoC.Base;
using BlockChain_PoC.Core.CommandResults;
using BlockChain_PoC.Core.Models.Dto;
using BlockChain_PoC.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.Commands
{
    public class GetBlockChainCommand : BaseCommand, IRequest<GetBlockChainResult>
    {
        public override DataTransferObjectType Type => DataTransferObjectType.GetBlockChain;
        public byte[]? LastBlockHash { get; set; }
        public long? Id { get; set; }
        public bool IsInclusive = true;
    }
    public class GetBlockChaimCommandHandler : IRequestHandler<GetBlockChainCommand, GetBlockChainResult>
    {
        private readonly IBlockChain _blockChain;
        public GetBlockChaimCommandHandler(IBlockChain blockChain)
        {
            _blockChain = blockChain;
        }
        public async Task<GetBlockChainResult> Handle(GetBlockChainCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<Block> blockChain = new List<Block>();
            Exception? caughtException = new Exception();
            try
            {
                if (request.Id.HasValue)
                {
                    blockChain = _blockChain.GetBlocksFromId(request.Id.Value, request.IsInclusive);
                }
                else if (request.LastBlockHash != null && request.LastBlockHash.Length > 0)
                {
                    blockChain = _blockChain.GetBlocksFromHash(request.LastBlockHash, request.IsInclusive);
                }
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }
            return new GetBlockChainResult()
            {
                Exception = caughtException,
                Blocks = blockChain.Select(x => BlockDto.ConvertToDto(x)).ToArray()
            };
        }
    }
}
