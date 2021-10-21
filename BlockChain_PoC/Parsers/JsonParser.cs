using BlockChain_PoC.Core;
using BlockChain_PoC.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Parsers
{
    public class JsonParser : IDataParser
    {
        public async Task<Type> GetCommandType(byte[] data)
        {
            var jsonString = Encoding.UTF8.GetString(data);
            var commnandType = System.Text.Json.JsonSerializer.Deserialize<BaseCommand>(jsonString);
            var type = DataTransferObject.TypeToDto[commnandType.Type];
            return type;
        }

        public async Task<BaseCommand> Parse<T>(byte[] data)
        {
            return await Parse(data, typeof(T));
        }

        public async Task<BaseCommand> Parse(byte[] data, Type type)
        {
            var jsonString = Encoding.UTF8.GetString(data);

            var commandToExecute = (BaseCommand)System.Text.Json.JsonSerializer.Deserialize(jsonString, type);
            return commandToExecute;
        }
    }
}
