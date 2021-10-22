using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Interfaces
{
    public interface IUserIO
    {
        public string GetUserTextInput();
        public void SendUserTextOutput(string text);
    }
}
