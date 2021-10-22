using Autofac;
using BlockChain_PoC.Base;
using BlockChain_PoC.Crypto;
using BlockChain_PoC.Interfaces;
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
        private readonly IBlockChain _blockChain;
        private readonly IUserIO _userIO;
        private readonly KeyPair _keyPair;
        public ConsoleActionsHandler(IBlockChain blockChain, IUserIO userIO, KeyPair keyPair)
        {
            _blockChain = blockChain;
            _userIO = userIO;
            _keyPair = keyPair;
        }
        public async Task StartActionsHandler()
        {
            while (true)
            {
                string actionCommand = _userIO.GetUserTextInput();
                string commandResult = await HandleCommand(actionCommand);
                _userIO.SendUserTextOutput(commandResult);
            }
        }
        private async Task<string> HandleCommand(string text)
        {
            var commonCommandRegex = new Regex(@"/(\S*) (.*)");
            var regexGroups = commonCommandRegex.Match(text).Groups;
            switch (regexGroups[1].Value)
            {
                case "addtransaction":
                    HandleAddTransactionCommand(regexGroups[2].Value);
                    break;
                default:
                    Console.WriteLine("Unknown command!");
                    break;
            }
            return string.Empty;
        }
        private async Task HandleAddTransactionCommand(string arguments)
        {
            #region Parameter parser
            var fromPattern = new Regex(@"(-f|--from) (\S*)");
            var from = fromPattern.Match(arguments).Groups.Values.LastOrDefault().Value;

            var toPattern = new Regex(@"(-t|--to) (\S*)");
            var to = toPattern.Match(arguments).Groups.Values.LastOrDefault().Value;

            var messagePattern = new Regex("(-m|--message) (\"|')(.*)(\"|')");
            var message = messagePattern.Match(arguments).Groups[3].Value;
            #endregion
            ITransaction messageTransaction = new MessageTransaction(from, to, Encoding.UTF8.GetBytes(message));
            messageTransaction.SignTransaction(_keyPair);
            _blockChain.AddPendingTransaction(messageTransaction);
            _blockChain.ProcessPendingTransactions();
        }
    }
}
