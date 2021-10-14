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
        public void MinePendingTransactions(string rewardAddress)
        {
            var block = Block.CreateBlock(this.pendingTransactions);
            AddBlock(block);
            this.pendingTransactions.Clear();
            this.pendingTransactions.Enqueue(new Transaction()
            {
                From = string.Empty,
                To = rewardAddress,
                Amount = Reward
            });
            if (!IsChainValid())
            {
                throw new Exception("Chain is invalid!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Chain is valid!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void CreateTransaction(Transaction transaction)
        {
            pendingTransactions.Enqueue(transaction);
        }
        public long GetAddressBalance(string address)
        {
            long balance = 0;
            foreach(var block in blocksStack)
            {
                foreach(var transaction in block.Transactions)
                {
                    if(transaction.To == address)
                    {
                        balance += transaction.Amount;
                    }
                    else if(transaction.From == address)
                    {
                        balance -= transaction.Amount;
                    }
                }
            }
            return balance;
        }
        private Block CreateGenesis()
        {
            var block = Block.CreateBlock(null, null, DateTime.MinValue);
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
