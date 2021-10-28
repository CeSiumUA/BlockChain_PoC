using BlockChain_PoC.Core.Commands;
using BlockChain_PoC.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.CommandResults
{
    public class GetBlockChainResult : BaseCommand
    {
        public BlockDto[]? Blocks { get; init; }
        public Exception? Exception { get; init; }
        public override DataTransferObjectType Type => DataTransferObjectType.GetBlockChainResult;
        //TODO
    }
}
