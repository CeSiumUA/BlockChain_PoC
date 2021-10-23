using BlockChain_PoC.Core.Commands;

namespace BlockChain_PoC.Parsers
{
    public interface IDataParser
    {
        public Task<Type> GetCommandType(byte[] data);
        public Task<BaseCommand?> Parse<T>(byte[] data);
        public Task<BaseCommand?> Parse(byte[] data, Type type);
    }
}
