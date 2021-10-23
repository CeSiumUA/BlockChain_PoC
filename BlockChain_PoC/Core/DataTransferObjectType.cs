using BlockChain_PoC.Core.Commands;
using BlockChain_PoC.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core
{
    public enum DataTransferObjectType
    {
        AddBlock,
        Other
    }
    public class DataTransferObject
    {
        public static Dictionary<DataTransferObjectType, Type> TypeToDto = new Dictionary<DataTransferObjectType, Type>()
        {
            {DataTransferObjectType.AddBlock, typeof(AddBlockCommand) }
        };
    }
}
