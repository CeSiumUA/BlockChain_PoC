namespace BlockChain_PoC.Interfaces
{
    public interface IUserIO
    {
        public string GetUserTextInput();
        public void SendUserTextOutput(string text);
        public void LogException(string message);
    }
}
