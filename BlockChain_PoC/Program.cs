using BlockChain_PoC;
using BlockChain_PoC.Base;

var blockChain = new BlockChain(true);

while (true)
{
    blockChain.CreateTransaction(new Transaction()
    {
        To = "ad1",
        From = "ad2",
        Amount = 100
    });
    blockChain.CreateTransaction(new Transaction()
    {
        To = "ad2",
        From = "ad1",
        Amount = 50
    });
    blockChain.MinePendingTransactions("Test1");
    blockChain.MinePendingTransactions("ad3");

    var balance = blockChain.GetAddressBalance("Test1");

    Console.WriteLine($"Balance for Test1 is {balance}");

    Console.ReadLine();
}