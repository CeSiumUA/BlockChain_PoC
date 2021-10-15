using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Crypto
{
    public record KeyPair
    {
        public byte[] PrivateKey { get; init; }
        public byte[] PublicKey { get; init; }
    }
    public static class KeyExtension
    {
        public static string GetHex(this byte[] byteArray)
        {
            return BitConverter.ToString(byteArray).Replace("-", string.Empty);
        }
        public static byte[] ToHexBytes(this string hex)
        {
            var bytes = Enumerable.Range(0, hex.Length / 2).Select(x => Convert.ToByte(hex.Substring(2 * x, 2), 16)).ToArray();
            return bytes;
        }
    }
}
