using BlockChain_PoC.Base;
using BlockChain_PoC.Core.Models.Dto;
using BlockChain_PoC.Interfaces;
using System.Text.Json;

namespace BlockChain_PoC.Core.Storage
{
    internal class TextFileStorage : IBlockChainStorage
    {
        private const string storageFileName = "blocks.json";
        public async Task<IEnumerable<Block>> LoadBlocks()
        {
            using (StreamReader sr = new StreamReader(storageFileName))
            {
                var json = await sr.ReadToEndAsync();
                var dtoList = JsonSerializer.Deserialize<BlockDto[]>(json);
                return dtoList?.Select(x => x.ConvertToBlock()) ?? new List<Block>();
            }
        }

        public async Task SaveBlocks(IEnumerable<Block> blocks, bool append = false)
        {
            await using (StreamWriter sw = new StreamWriter(storageFileName, append))
            {
                var dtoList = blocks.Select(x => BlockDto.ConvertToDto(x));
                var json = JsonSerializer.Serialize(dtoList);
                await sw.WriteAsync(json);
            }
        }
    }
}
