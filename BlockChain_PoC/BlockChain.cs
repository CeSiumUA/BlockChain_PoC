using BlockChain_PoC.Base;
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
        private Queue<Transaction> pendingTransactions = new Queue<Transaction>();
        private const int Reward = 100;
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
        public void MinePendingTransactions()
        {

        }
        private Block CreateGenesis()
        {
            var block = Block.CreateBlock("The very first genesis block", null, DateTime.MinValue);
            return block;
        }
        public bool IsChainValid()
        {
            var blockList = blocksStack.Reverse().ToArray();
            for(int x = 1; x < blocksStack.Count; x++)
            {
                var currentBlock = blockList[x];
                var previousBlock = blockList[x - 1];
                
                if(!Enumerable.SequenceEqual<byte>(currentBlock.Hash, currentBlock.CreateHash()))
                {
                    return false;
                }

                if(!Enumerable.SequenceEqual<byte>(currentBlock.PreviousHash, previousBlock.Hash))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
