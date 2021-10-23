namespace BlockChain_PoC.Core.Commands
{
    public class BaseCommand
    {
        public BaseCommand()
        {

        }
        public virtual DataTransferObjectType Type { get; }
    }
}
