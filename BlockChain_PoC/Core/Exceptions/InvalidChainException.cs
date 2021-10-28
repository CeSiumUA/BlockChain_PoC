﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core.Exceptions
{
    public class InvalidChainException : Exception
    {
        public override string Message => "Chain could not be verified!";
    }
}