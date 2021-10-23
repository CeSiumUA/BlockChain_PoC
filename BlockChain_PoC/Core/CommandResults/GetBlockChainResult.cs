using BlockChain_PoC.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.CommandResults
{
    public record GetBlockChainResult
    {
        public BlockDto[]? Blocks { get; init; }
        public Exception? Exception { get; init; }
    }
}
