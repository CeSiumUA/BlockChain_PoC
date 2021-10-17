using BlockChain_PoC.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Interfaces
{
    public interface ICommandsExecuter
    {
        public Task<ICommand> Execute(ICommand command, Action<byte[]> responseWriter = null);
    }
}
