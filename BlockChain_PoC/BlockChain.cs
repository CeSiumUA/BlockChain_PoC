using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC
{
    internal class BlockChain
    {
        private Stack<Block> blocksStack = new Stack<Block>();
        public BlockChain(bool createGenesis = false)
        {
            if (createGenesis)
            {
                var genesisBlock = CreateGenesis();
                blocksStack.Push(genesisBlock);
            }
        }
        public void AddBlock(Block block)
        {
            var lastBlock = blocksStack.Peek();
            if (lastBlock != null)
            {
                block.PreviousHash = lastBlock.Hash;
            }
            blocksStack.Push(block);
        }
        private Block CreateGenesis()
        {
            var block = new Block()
            {
                CreatedOn = DateTime.MinValue,
                Data = Encoding.Unicode.GetBytes("The very first genesis block"),
            };
            return block;
        }
    }
}
