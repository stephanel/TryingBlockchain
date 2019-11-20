using Blockchain.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Blockchain.Out.P2P
{
    class Program
    {
        static string ExitCode = "6";
        public static Core.Blockchain Blockchain = new Core.Blockchain();
        static P2PClient client = new P2PClient();
        static P2PServer server = null;
        public static int Port { get; set; } = 0;
        public static string Me { get; set; } = "Unknown";

        //static IDictionary<string, P2PClient> clientsDict = new Dictionary<string, P2PClient>();

        static void Main(string[] args)
        {
            if(args.Length > 0)
            {
                Port = int.Parse(args[0]);
                Me = args[1];
            }

            if(Port > 0)
            {
                server = new P2PServer();
                server.Start();
            }

            var input = PromptMenu();

            while(input != ExitCode)
            {
                switch(input)
                {
                    case "1":
                        // connect to a server
                        var url = PromptForUrl();

                        client.Connect($"{url}/Blockchain");

                        break;

                    case "2":
                        // add a transaction
                        var toAddress = PromptForTransactionReceiver();
                        var amount = PromptForTransactionAmount();

                        Blockchain.CreateTransaction(new Transaction(Me, toAddress, amount));
                        Blockchain.ProcessPendingTransactions(Me);

                        string data = JsonConvert.SerializeObject(Blockchain);

                        client.Broadcast(data);

                        break;

                    case "3":
                        DisplayBlockchain();
                        break;

                    case "4":
                        CheckBlockchainValidity();
                        break;

                    case "5":
                        DisplayBalances();
                        break;
                }

                input = PromptMenu();
            }
        }

        static string PromptMenu()
        {
            Console.WriteLine("==================");
            Console.WriteLine("1. Connect to a server");
            Console.WriteLine("2. Add a transaction");
            Console.WriteLine("3. Display Blockchain");
            Console.WriteLine("4. Check blockchain validity");
            Console.WriteLine("5. Get balances");
            Console.WriteLine($"{ExitCode}. Exit");
            Console.WriteLine("==================");
            Console.WriteLine("Please select an action");
            var keyInfo = Console.ReadKey();

            Console.WriteLine();

            return keyInfo.KeyChar.ToString();
        }

        static string PromptForUrl()
        {
            Console.WriteLine("Enter the url you want to connect to: ");
            return Console.ReadLine();
        }

        static string PromptForTransactionReceiver()
        {
            Console.WriteLine("Please enter the receiver name: ");
            return Console.ReadLine();
        }
       
        static int PromptForTransactionAmount()
        {
            var input = "";
            int amount = 0;

            while(!int.TryParse(input, out amount))
            {
                Console.WriteLine("Please enter the amount: ");
                input = Console.ReadLine();
            }

            return amount;
        }

        static void DisplayBlockchain()
        {
            var blockchain = JsonConvert.SerializeObject(Blockchain, Formatting.Indented);

            Console.WriteLine(blockchain);
        }

        static void CheckBlockchainValidity()
        {
            var isValid = Blockchain.IsValid();

            Console.WriteLine($"Blockchain is valid? {isValid}");
        }

        static void DisplayBalances()
        {
            foreach (var wallet in Blockchain.GetWallets())
            {
                Console.WriteLine($"{wallet}' balance: {Blockchain.GetBalance(wallet)}");
            }
        }
    }
}
