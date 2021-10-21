using BlockChain_PoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.Exceptions
{
    public class InvalidTransactionException : Exception
    {
        public InvalidTransactionException(ITransaction transaction) : base($"Transaction with Id: {transaction.Id} is invalid")
        {

        }
    }
}
