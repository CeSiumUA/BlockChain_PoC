using BlockChain_PoC.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Interfaces
{
    public interface INetworkInterface : IDisposable
    {
        public Task Init();
        public Task Broadcast(byte[] data);
        public Task Broadcast<T>(T data);
    }
}
