using BlockChain_PoC.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core
{
    public static class Extensions
    {
        public static string GetHex(this byte[] byteArray)
        {
            return BitConverter.ToString(byteArray).Replace("-", string.Empty);
        }
        public static byte[] ToHexBytes(this string hex)
        {
            var bytes = Enumerable.Range(0, hex.Length / 2).Select(x => Convert.ToByte(hex.Substring(2 * x, 2), 16)).ToArray();
            return bytes;
        }
        public static bool IsChainValid(this IEnumerable<Block> blocks)
        {
            var blocksList = blocks.ToList();
            if(blocksList.Count == 1)
            {
                return blocksList.First().IsValid();
            }
            for(int x = 1; x < blocksList.Count; x++)
            {
                var currentBlock = blocksList[x];
                var previousBlock = blocksList[x - 1];
                if(!currentBlock.IsValid() || !previousBlock.IsValid())
                {
                    return false;
                }
                if(!Enumerable.SequenceEqual(currentBlock.PreviousHash, previousBlock.Hash))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
