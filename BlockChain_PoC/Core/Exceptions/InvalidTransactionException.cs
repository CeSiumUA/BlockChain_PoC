using BlockChain_PoC.Interfaces;

namespace BlockChain_PoC.Core.Exceptions
{
    public class InvalidTransactionException : Exception
    {
        public InvalidTransactionException(ITransaction transaction) : base($"Transaction with Id: {transaction.Id} is invalid")
        {

        }
    }
}
