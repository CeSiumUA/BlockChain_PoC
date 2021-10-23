using BlockChain_PoC.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Interfaces
{
    public interface IBlockChainStorage
    {
        public Task SaveBlocks(IEnumerable<Block> blocks, bool append = false);
        public Task<IEnumerable<Block>> LoadBlocks();
    }
}
