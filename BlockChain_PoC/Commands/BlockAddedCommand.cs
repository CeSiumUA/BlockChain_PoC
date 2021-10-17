using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Commands
{
    public class BlockAddedCommand : ICommand
    {
        public CommandType Type => CommandType.BlockAdded;

        public Task ExecuteAsync(Action<byte[]> responseWriter)
        {
            throw new NotImplementedException();
        }
    }
}
