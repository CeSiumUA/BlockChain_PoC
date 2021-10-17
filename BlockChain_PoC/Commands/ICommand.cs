using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Commands
{
    public interface ICommand
    {
        public CommandType Type { get; }
        public Task ExecuteAsync(Action<byte[]> responseWriter = null);
    }

}
