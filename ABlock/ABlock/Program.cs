using System;
using System.Text.Json;

namespace ABlock
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            BlockChains bc = new BlockChains();
            bc.addBlockData("hello");
            bc.addBlockData("WangPlus");
            bc.isBlockValid(1);
            bc.isBlockValid(2);

            var block_chains = bc.getBlockChains();
            for (int i = 0; i < block_chains.Count; i++)
            {
                Console.WriteLine($"");
                Console.WriteLine($"Node {i + 1}");
                Console.WriteLine($"Pre-Hash: {block_chains[i + 1].pre_hash}");
                string block_data = JsonSerializer.Serialize(block_chains[i + 1].data);
                Console.WriteLine($"Data: {block_data}");
                Console.WriteLine($"Hash: {block_chains[i + 1].hash}");
            }
        }
    }
}
