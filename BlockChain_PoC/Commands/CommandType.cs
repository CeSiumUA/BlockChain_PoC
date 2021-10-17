using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Commands
{
    public enum CommandType
    {
        BlockAdded
    }
    public static class Commands
    {
        public static Dictionary<CommandType, Type> CommandsDictionary = new Dictionary<CommandType, Type>()
        {
            { CommandType.BlockAdded, typeof(BlockAddedCommand)}
        };
    }
}
