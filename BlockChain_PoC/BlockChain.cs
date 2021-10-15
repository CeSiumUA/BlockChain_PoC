using BlockChain_PoC.Base;
using BlockChain_PoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC
{
    internal class BlockChain : IValidatable
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
        private void AddBlock(Block block)
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
            this.pendingTransactions.Enqueue(new Transaction()
            {
                From = string.Empty,
                To = rewardAddress,
                Amount = Reward
            });
            var block = Block.CreateBlock(this.pendingTransactions);
            AddBlock(block);
            this.pendingTransactions.Clear();
            if (!IsValid())
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
        public void AddTransaction(Transaction transaction)
        {
            if(string.IsNullOrEmpty(transaction.To) || string.IsNullOrEmpty(transaction.From))
            {
                throw new ArgumentNullException("From and To addresses must be filled!");
            }

            if (!transaction.IsValid())
            {
                throw new InvalidOperationException("Transaction is invalid!");
            }

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
        public bool IsValid()
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

                if (!currentBlock.IsValid())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
