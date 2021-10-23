using BlockChain_PoC.Interfaces;

namespace BlockChain_PoC.Core
{
    public class ConsoleUserIO : IUserIO
    {
        public string GetUserTextInput()
        {
            return Console.ReadLine() ?? string.Empty;
        }

        public void LogException(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void SendUserTextOutput(string text)
        {
            Console.Write(string.IsNullOrEmpty(text) ? string.Empty : $"{text}{Environment.NewLine}");
        }
    }
}
