using BlockChain_PoC.Core.Commands;
using BlockChain_PoC.Core.Models.Dto;
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
        public Task<BaseCommand> Parse<T>(byte[] data);
        public Task<BaseCommand> Parse(byte[] data, Type type);
    }
}
