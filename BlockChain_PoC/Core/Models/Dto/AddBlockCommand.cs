using BlockChain_PoC.Base;
using BlockChain_PoC.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.Models.Dto
{
    public class AddBlockCommand : BaseCommand, IRequest<Block>
    {
        public override DataTransferObjectType Type => DataTransferObjectType.AddBlock;
        public Block AddedBlock { get; set; }
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
            if(request.AddedBlock == null)
            {
                throw new ArgumentNullException();
            }
            return _blockchain.AddBlock(request.AddedBlock);
        }
    }
}
