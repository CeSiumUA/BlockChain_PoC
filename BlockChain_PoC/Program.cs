using BlockChain_PoC;
using BlockChain_PoC.Base;

var blockChain = new BlockChain(true);

while (true)
{
    Console.WriteLine("Enter a value: ");
    var blockString = Console.ReadLine();
    if(blockString == "checkvalid")
    {
        Console.WriteLine(blockChain.IsChainValid());
    }
    var block = Block.CreateBlock(blockString);
    blockChain.AddBlock(block);
}