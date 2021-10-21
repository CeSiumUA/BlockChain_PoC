﻿using BlockChain_PoC.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Interfaces
{
    public interface IPeerProvider
    {
        public Task<IEnumerable<NetworkMember>> GetPeersAsync();
    }
}