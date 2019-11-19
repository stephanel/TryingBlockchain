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

            blockchain.CreateTransaction(new Transaction("Henry", "MaHesh", 10));
            blockchain.ProcessPendingTransactions("Bill");
            Console.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));

            blockchain.CreateTransaction(new Transaction("MaHesh", "Henry", 5));
            blockchain.CreateTransaction(new Transaction("MaHesh", "Henry", 5));
            blockchain.ProcessPendingTransactions("Bill");

            var endTime = DateTime.Now;

            Console.WriteLine($"Duration: {endTime - startTime}");

            Console.WriteLine("=========================");
            Console.WriteLine($"Henry' balance: {blockchain.GetBalance("Henry")}");
            Console.WriteLine($"MaHesh' balance: {blockchain.GetBalance("MaHesh")}");
            Console.WriteLine($"Bill' balance: {blockchain.GetBalance("Bill")}");

            Console.WriteLine("=========================");
            Console.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));

            Console.WriteLine($"Is Chain Valid: {blockchain.IsValid()}");


        }
    }
}
