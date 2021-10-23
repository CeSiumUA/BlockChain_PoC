namespace BlockChain_PoC.Crypto
{
    public record KeyPair
    {
        public byte[] PrivateKey { get; init; } = new byte[0];
        public byte[] PublicKey { get; init; } = new byte[0];
    }
}
