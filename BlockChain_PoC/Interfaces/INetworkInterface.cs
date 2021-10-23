namespace BlockChain_PoC.Interfaces
{
    public interface INetworkInterface : IDisposable
    {
        public Task Init();
        public Task Broadcast(byte[] data);
        public Task Broadcast<T>(T data);
    }
}
