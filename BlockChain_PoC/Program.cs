using BlockChain_PoC;
using BlockChain_PoC.Base;
using BlockChain_PoC.Crypto;
using System.Text;

var blockChain = new BlockChain(true);

var keyPair = KeyGen.LoadKey();

string walletAddress = keyPair.PublicKey.GetHex();

while (true)
{
    var transaction = new Transaction()
    {
        Amount = 10,
        From = walletAddress,
        To = "test1",
    };
    transaction.SignTransaction(keyPair);
    blockChain.AddTransaction(transaction);
    blockChain.MinePendingTransactions(walletAddress);

    Console.WriteLine($"Current wallet balance: {blockChain.GetAddressBalance(walletAddress)}");

    Console.ReadLine();
}