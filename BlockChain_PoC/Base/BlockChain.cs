using BlockChain_PoC.Core;
using BlockChain_PoC.Core.Commands;
using BlockChain_PoC.Core.Exceptions;
using BlockChain_PoC.Core.Models.Dto;
using BlockChain_PoC.Interfaces;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BlockChain_PoC.Base
{
    public class BlockChain : IBlockChain
    {
        private ObservableCollection<Block> Blocks { get; set; } = new ObservableCollection<Block>();
        public Queue<ITransaction> TransactionsQueue { get; set; } = new Queue<ITransaction>();
        private object _queueLock = new object();
        private readonly INetworkInterface _network;
        private readonly IBlockChainStorage _blockChainStorage;
        public BlockChain(INetworkInterface networkInterface, IBlockChainStorage blockChainStorage)
        {
            _network = networkInterface;
            _blockChainStorage = blockChainStorage;
            Blocks = new ObservableCollection<Block>(_blockChainStorage.LoadBlocks().Result);
            Blocks.CollectionChanged += HandleBlocksCollectionChanged;
        }

        private void HandleBlocksCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _blockChainStorage.SaveBlocks(this.Blocks);
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
                var lastBlockHash = GetLatestBlock()?.Hash ?? new byte[0];
                bool CheckPreviousBlockHashEquality()
                {
                    return Enumerable.SequenceEqual<byte>(block.PreviousHash, lastBlockHash);
                }
                if (!CheckPreviousBlockHashEquality())
                {
                    return block;
                }
                if (!block.IsMined)
                {
                    block.MineBlock();
                }
                if (block.IsValid() && CheckPreviousBlockHashEquality())
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
            foreach (var transaction in transactions)
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
            var block = new Block(transactions, lastBlock?.Id + 1 ?? 0, lastBlock?.Hash ?? new byte[0], 2);
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
                while (TransactionsQueue.Count > 0)
                {
                    transactions.Add(TransactionsQueue.Dequeue());
                }
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
                AddedBlock = BlockDto.ConvertToDto(block),
            };
            _network.Broadcast(addBlockCommand);
        }

        public IEnumerable<Block> GetBlocksFromId(long id)
        {
            var block = this.Blocks.FirstOrDefault(x => x.Id == id);
            if(block == null)
            {
                throw new IndexNotFoundException(id);
            }
            var blockIndex = this.Blocks.IndexOf(block);
            var blocksBatch = this.Blocks.Skip(blockIndex + 1);
            if (!blocksBatch.IsChainValid())
            {
                throw new InvalidChainException();
            }
            return blocksBatch;
        }

        public IEnumerable<Block> GetBlocksFromHash(byte[] hash)
        {
            var block = this.Blocks.FirstOrDefault(x => Enumerable.SequenceEqual<byte>(hash, x.Hash));
            if (block == null)
            {
                throw new HashNotFoundException(hash);
            }
            var blockIndex = this.Blocks.IndexOf(block);
            var blocksBatch = this.Blocks.Skip(blockIndex + 1);
            if (!blocksBatch.IsChainValid())
            {
                throw new InvalidChainException();
            }
            return blocksBatch;
        }
    }
}
