using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Network
{
    public class NetworkMember
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        //TODO delete this boilerplate!!!
        public bool IsMine { get; set; }
    }
}
