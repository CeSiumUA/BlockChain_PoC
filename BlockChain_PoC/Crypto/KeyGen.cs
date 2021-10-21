using BlockChain_PoC.Core;
using Secp256k1Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_PoC.Crypto
{
    public class KeyGen
    {
        public static KeyPair CreateKeyPair()
        {
            using(var secp256k1 = new Secp256k1())
            {
                var privateKey = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    do
                    {
                        rng.GetBytes(privateKey);
                    }
                    while(!secp256k1.SecretKeyVerify(privateKey));
                }
                var publickKey = new byte[64];
                if(!secp256k1.PublicKeyCreate(publickKey, privateKey))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error creating a key!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                return new KeyPair()
                {
                    PrivateKey = privateKey,
                    PublicKey = publickKey
                };
            }
        }
        public static KeyPair LoadKey(string path = null, bool createIfNotExists = false)
        {
            path = path ?? "key.dat";
            if (!File.Exists(path) && !createIfNotExists)
            {
                throw new FileNotFoundException($"File with key pair: {path} was not found!");
            }
            if (!File.Exists(path) && createIfNotExists)
            {
                var generatedkeypair = CreateKeyPair();
                string textToWrite = $"{generatedkeypair.PrivateKey.GetHex()}{Environment.NewLine}{generatedkeypair.PublicKey.GetHex()}";
                File.WriteAllText(path, textToWrite);
            }
            var fileText = File.ReadAllLines(path);

            var keyPair = new KeyPair()
            {
                PrivateKey = fileText[0].ToHexBytes(),
                PublicKey = fileText[1].ToHexBytes()
            };
            return keyPair;
        }
    }
}
