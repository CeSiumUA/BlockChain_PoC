using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.Exceptions
{
    public class HashNotFoundException : Exception
    {
        public HashNotFoundException(byte[] hash) : base($"Specified hash: {hash.GetHex()} was not found!")
        {

        }
    }
}
