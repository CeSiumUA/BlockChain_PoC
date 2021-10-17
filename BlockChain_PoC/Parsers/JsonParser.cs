using BlockChain_PoC.Commands;
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
            var commnandType = System.Text.Json.JsonSerializer.Deserialize<ICommand>(jsonString);
            var type = Commands.Commands.CommandsDictionary[commnandType.Type];
            return type;
        }

        public async Task<ICommand> Parse<T>(byte[] data)
        {
            return await Parse(data, typeof(T));
        }

        public async Task<ICommand> Parse(byte[] data, Type type)
        {
            var jsonString = Encoding.UTF8.GetString(data);

            var commandToExecute = System.Text.Json.JsonSerializer.Deserialize(jsonString, type) as ICommand;
            return commandToExecute;
        }
    }
}
