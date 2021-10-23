using BlockChain_PoC.Network;

namespace BlockChain_PoC.Interfaces
{
    public interface IPeerProvider
    {
        public Task<IEnumerable<NetworkMember>> GetPeersAsync();
    }
}
