using BlockChain_PoC.Base;
using BlockChain_PoC.Core.Models.Dto;
using BlockChain_PoC.Interfaces;
using MediatR;

namespace BlockChain_PoC.Core.Commands
{
    public class AddBlockCommand : BaseCommand, IRequest<Block>
    {
        public override DataTransferObjectType Type => DataTransferObjectType.AddBlock;
        public BlockDto? AddedBlock { get; set; }
    }
    public class AddBlockCommandHanlder : IRequestHandler<AddBlockCommand, Block>
    {
        private readonly IBlockChain _blockchain;
        public AddBlockCommandHanlder(IBlockChain blockChain)
        {
            _blockchain = blockChain;
        }
        public async Task<Block> Handle(AddBlockCommand request, CancellationToken cancellationToken)
        {
            if (request.AddedBlock == null)
            {
                throw new ArgumentNullException();
            }
            return _blockchain.AddBlock(request.AddedBlock.ConvertToBlock());
        }
    }
}
