using BlockChain_PoC.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Interfaces
{
    public interface IBlockChain : IValidatable
    {
        public Block? GetLatestBlock();
        public Block AddBlock(Block block);
        public void AddBlocks(IEnumerable<Block> blocks);
        public void AddPendingTransaction(ITransaction transaction);
        public void AddPendingTransactions(IEnumerable<ITransaction> transactions);
        public Block GetPendingTransactionsBlock(int? batchSize = null);
        public Block ProcessPendingTransactions(int? batchSize = null);
        public ITransaction PopTransaction();
    }
}
