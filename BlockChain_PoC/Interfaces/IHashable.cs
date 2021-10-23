namespace BlockChain_PoC.Interfaces
{
    public interface IHashable : IValidatable
    {
        public byte[] GetHash();
    }
}
