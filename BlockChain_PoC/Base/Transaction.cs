using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Base
{
    [MessagePackObject]
    public record Transaction
    {
        [Key(0)]
        public string From { get; init; }
        [Key(1)]
        public string To { get; init; }
        [Key(2)]
        public long Amount { get; init; }
    }
}
