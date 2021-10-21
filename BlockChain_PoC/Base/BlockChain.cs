using BlockChain_PoC.Core;
using BlockChain_PoC.Core.Exceptions;
using BlockChain_PoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Base
{
    public class BlockChain : IBlockChain
    {
        private List<Block> Blocks { get; set; } = new List<Block>();
        public Queue<ITransaction> TransactionsQueue { get; set; } = new Queue<ITransaction>();
        private object _queueLock = new object();
        private INetworkInterface _network;
        public BlockChain(INetworkInterface networkInterface)
        {
            _network = networkInterface;
        }
        #region AddGetBlock
        public Block? GetLatestBlock()
        {
            return Blocks.LastOrDefault();
        }
        public Block AddBlock(Block block)
        {
            if (!block.IsMined)
            {
                block.MineBlock();
            }
            var lastBlockHash = GetLatestBlock()?.Hash ?? new byte[0];
            if (block.IsValid() && Enumerable.SequenceEqual<byte>(block.PreviousHash, lastBlockHash))
            {
                Blocks.Add(block);
            }
            return block;
        }
        public void AddBlocks(IEnumerable<Block> blocks)
        {
            if (blocks.IsChainValid())
            {
                foreach (var block in blocks)
                {
                    AddBlock(block);
                }
            }
        }
        #endregion
        #region AddGetTransactions
        public void AddPendingTransaction(ITransaction transaction)
        {
            if (!transaction.IsValid())
            {
                throw new InvalidTransactionException(transaction);
            }
            lock (_queueLock)
            {
                TransactionsQueue.Enqueue(transaction);
            }
        }

        public void AddPendingTransactions(IEnumerable<ITransaction> transactions)
        {
            foreach(var transaction in transactions)
            {
                AddPendingTransaction(transaction);
            }
        }

        public ITransaction PopTransaction()
        {
            return TransactionsQueue.Dequeue();
        }
        #endregion

        public bool IsValid()
        {
            return this.Blocks.IsChainValid();
        }
    }
}
