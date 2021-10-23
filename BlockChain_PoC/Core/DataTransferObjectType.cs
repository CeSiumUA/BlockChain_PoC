using BlockChain_PoC.Core.Commands;

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
