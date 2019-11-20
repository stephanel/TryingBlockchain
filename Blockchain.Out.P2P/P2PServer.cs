using Blockchain.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Blockchain.Out.P2P
{
    public class P2PServer : WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer wss = null;

        public void Start()
        {
            var url = $"ws://127.0.0.1:{Program.Port}";

            wss = new WebSocketServer(url);
            wss.AddWebSocketService<P2PServer>("/Blockchain");
            wss.Start();

            Console.WriteLine($"Started server at {url}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if(e.Data == "Hi Server")
            {
                Console.WriteLine(e.Data);
                Send("Hi Client");
            }
            else
            {
                Core.Blockchain newChain = JsonConvert.DeserializeObject<Core.Blockchain>(e.Data);

                if(newChain.IsValid() && newChain.Chain.Count > Program.Blockchain.Chain.Count)
                {
                    List<Transaction> newTransactions = new List<Transaction>();
                    newTransactions.AddRange(newChain.PendingTransactions);
                    newTransactions.AddRange(Program.Blockchain.PendingTransactions);

                    newChain.PendingTransactions = newTransactions;
                    Program.Blockchain = newChain;
                }

                if(!chainSynched)
                {
                    Send(JsonConvert.SerializeObject(Program.Blockchain));
                    chainSynched = true;
                }
            }
        }
    }
}
