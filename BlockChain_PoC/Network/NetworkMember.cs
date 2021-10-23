namespace BlockChain_PoC.Network
{
    public class NetworkMember
    {
        public string IPAddress { get; set; } = string.Empty;
        public int Port { get; set; }
        //TODO delete this boilerplate!!!
        public bool IsMine { get; set; }
    }
}
