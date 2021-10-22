using BlockChain_PoC.Core;
using BlockChain_PoC.Core.Exceptions;
using BlockChain_PoC.Core.Models.Dto;
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
            if (!Blocks.Any(x => x.Id == block.Id))
            {
                if (!block.IsMined)
                {
                    block.MineBlock();
                }
                var lastBlockHash = GetLatestBlock()?.Hash ?? new byte[0];
                if (block.IsValid() && Enumerable.SequenceEqual<byte>(block.PreviousHash, lastBlockHash))
                {
                    Blocks.Add(block);
                    BroadcastAddBlockCommand(block);
                }
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

        public Block GetPendingTransactionsBlock(int? batchSize = null)
        {
            var transactions = RetreiveTransactions(batchSize);
            var lastBlock = GetLatestBlock();
            var block = new Block(transactions, lastBlock?.Id ?? 0, lastBlock?.PreviousHash ?? new byte[0]);
            return block;
        }

        public Block ProcessPendingTransactions(int? batchSize = null)
        {
            var pendingTransactionBlock = GetPendingTransactionsBlock(batchSize);
            return AddBlock(pendingTransactionBlock);
        }

        private IEnumerable<ITransaction> RetreiveTransactions(int? batchSize = null)
        {
            List<ITransaction> transactions = new List<ITransaction>();
            if (!batchSize.HasValue)
            {
                transactions = this.TransactionsQueue.ToList();
            }
            else
            {
                int dequeued = 0;
                while (TransactionsQueue.Count > 0 && dequeued < batchSize.Value)
                {
                    transactions.Add(TransactionsQueue.Dequeue());
                    dequeued++;
                }
            }
            return transactions;
        }
        private void BroadcastAddBlockCommand(Block block)
        {
            var addBlockCommand = new AddBlockCommand()
            {
                AddedBlock = block,
            };
            _network.Broadcast(addBlockCommand);
        }
    }
}
