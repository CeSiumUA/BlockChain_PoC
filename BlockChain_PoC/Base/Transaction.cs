using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Base
{
    internal record Transaction
    {
        public string From { get; init; }
        public string To { get; init; }
        public long Amount { get; init; }
    }
}
