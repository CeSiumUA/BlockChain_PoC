using BlockChain_PoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Core
{
    public class ConsoleUserIO : IUserIO
    {
        public string GetUserTextInput()
        {
            return Console.ReadLine();
        }

        public void SendUserTextOutput(string text)
        {
            Console.Write(string.IsNullOrEmpty(text) ? string.Empty : $"{Environment.NewLine}{text}");
        }
    }
}
