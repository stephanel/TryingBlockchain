using Blockchain;
using Blockchain.Core;
using Newtonsoft.Json;
using System;

namespace Blockchain.Out
{
    class Program
    {
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;

            Core.Blockchain blockchain = new Core.Blockchain();

            blockchain.AddBlock(new Block(DateTime.Now, 
                null, "{sender:Henry,receiver:MaHesh,amount:10}"));
            blockchain.AddBlock(new Block(DateTime.Now,
                null, "{sender:MaHesh,receiver:Henry,amount:5}"));
            blockchain.AddBlock(new Block(DateTime.Now,
                null, "{sender:Mahesh,receiver:Henry,amount:5}"));

            var endTime = DateTime.Now;

            Console.WriteLine($"Duration: {endTime - startTime}");
            Console.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));

            Console.WriteLine($"Is Chain Valid: {blockchain.IsValid()}");


        }
    }
}
