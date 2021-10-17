using BlockChain_PoC.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Parsers
{
    public interface IDataParser
    {
        public Task<Type> GetCommandType(byte[] data);
        public Task<ICommand> Parse<T>(byte[] data);
        public Task<ICommand> Parse(byte[] data, Type type);
    }
}
