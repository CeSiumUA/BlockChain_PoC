using BlockChain_PoC.Base;

namespace BlockChain_PoC.Interfaces
{
    public interface IBlockChainStorage
    {
        public Task SaveBlocks(IEnumerable<Block> blocks, bool append = false);
        public Task<IEnumerable<Block>> LoadBlocks();
    }
}
