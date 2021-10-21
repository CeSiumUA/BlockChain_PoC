using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.Models.Dto
{
    public abstract class BaseCommand
    {
        public abstract DataTransferObjectType Type { get; }
    }
}
