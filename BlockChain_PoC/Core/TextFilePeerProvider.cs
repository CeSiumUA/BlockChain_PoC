using BlockChain_PoC.Interfaces;
using BlockChain_PoC.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core
{
    public class TextFilePeerProvider : IPeerProvider
    {
        private const string fileName = "Network/peers.json";
        public async Task<IEnumerable<NetworkMember>> GetPeersAsync()
        {
            var json = await File.ReadAllTextAsync($"{Environment.CurrentDirectory}/{fileName}");
            var networkMembers = JsonSerializer.Deserialize<NetworkMember[]>(json);
            return networkMembers ?? new NetworkMember[0];
        }
    }
}
