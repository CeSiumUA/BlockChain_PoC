using BlockChain_PoC;

var blockChain = new BlockChain(true);

while (true)
{
    Console.WriteLine("Enter a value: ");
    var blockString = Console.ReadLine();
    var block = Block.CreateBlock(blockString);
    blockChain.AddBlock(block);
}