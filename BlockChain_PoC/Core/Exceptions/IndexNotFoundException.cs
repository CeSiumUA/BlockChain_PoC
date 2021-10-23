using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.Exceptions
{
    public class IndexNotFoundException : Exception
    {
        public IndexNotFoundException(long id): base($"Specified index: {id} was not found!")
        {

        }
    }
}
