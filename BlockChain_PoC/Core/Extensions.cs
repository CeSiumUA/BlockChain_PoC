using BlockChain_PoC.Base;
using System.Runtime;
using System.Diagnostics.CodeAnalysis;

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
            if (blocksList.Count == 1)
            {
                return blocksList.First().IsValid();
            }
            for (int x = 1; x < blocksList.Count; x++)
            {
                var currentBlock = blocksList[x];
                var previousBlock = blocksList[x - 1];
                if (!currentBlock.IsValid() || !previousBlock.IsValid())
                {
                    return false;
                }
                if (!Enumerable.SequenceEqual(currentBlock.PreviousHash, previousBlock.Hash))
                {
                    return false;
                }
            }
            return true;
        }
        public static IEnumerable<Block> SelectMoreDifficultChain(this IEnumerable<Block> blocks1, IEnumerable<Block> blocks2)
        {
            double blocks1Difficulty = blocks1.Select(x => Math.Pow(2, x.Difficulty)).Sum();
            double blocks2Difficulty = blocks2.Select(x => Math.Pow(2, x.Difficulty)).Sum();

            if(blocks2Difficulty > blocks1Difficulty)
            {
                return blocks2;
            }
            else
            {
                return blocks1;
            }
        }
        public static IEnumerable<Block> MergeChain(this IEnumerable<Block> blocks1, IEnumerable<Block> blocks2)
        {
            Block? innerIntersection = null;
            var blockComparer = new BlocksEqualityComparer();
            var firstCommonBlock = blocks1.Intersect(blocks2, blockComparer).FirstOrDefault();
            if(firstCommonBlock == null)
            {
                if(Enumerable.SequenceEqual(blocks2.First().PreviousHash, blocks1.Last().Hash))
                {
                    var blocksList = new List<Block>();
                    foreach(var block in blocks1)
                    {
                        blocksList.Add(block);
                    }
                    foreach(var block in blocks2)
                    {
                        blocksList.Add(block);
                    }
                    return blocksList;
                }
                else if(Enumerable.SequenceEqual(blocks2.Last().Hash, blocks1.First().PreviousHash))
                {
                    var blocksList = new List<Block>();
                    foreach (var block in blocks2)
                    {
                        blocksList.Add(block);
                    }
                    foreach (var block in blocks1)
                    {
                        blocksList.Add(block);
                    }
                    return blocksList;
                }
            }

            var blocks1Index = Array.IndexOf(blocks1.ToArray(), firstCommonBlock);
            var blocks2Index = Array.IndexOf(blocks2.ToArray(), firstCommonBlock);
            
            if(blocks1Index == -1 || blocks2Index == -1)
            {
                throw new Exception("Unknown strange error!");
            }

            var difficultChain = SelectMoreDifficultChain(blocks1.Skip(blocks1Index), blocks2.Skip(blocks2Index));

            //TODO

            var str = File.ReadAllText("tttt");

            return blocks1;
        }
    }
    public class BlocksEqualityComparer : IEqualityComparer<Block>
    {
        public bool Equals(Block? x, Block? y)
        {
            if(x != null && y != null)
            {
                return x.Id == y.Id && Enumerable.SequenceEqual(x.Hash, y.Hash);
            }
            return x == y;
        }

        public int GetHashCode([DisallowNull] Block obj)
        {
            return obj.GetHashCode();
        }
    }
}
