using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlockChain_PoC
{
    public class ConsoleActionsHandler
    {
        public ConsoleActionsHandler()
        {

        }
        public async Task StartActionsHandler()
        {
            while (true)
            {
                string actionCommand = Console.ReadLine();
                string commandResult = await HandleCommand(actionCommand);
                Console.Write(string.IsNullOrEmpty(commandResult) ? string.Empty : $"{Environment.NewLine}{commandResult}");
            }
        }
        private async Task<string> HandleCommand(string text)
        {
            var commonCommandRehex = new Regex("(/.*) (.*)");
            return string.Empty;
        }
    }
}
