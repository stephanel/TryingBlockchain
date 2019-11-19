using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockchain.Core
{
    // https://www.c-sharpcorner.com/article/blockchain-basics-building-a-blockchain-in-net-core/
    // https://www.c-sharpcorner.com/article/building-a-blockchain-in-net-core-proof-of-work/
    // https://www.c-sharpcorner.com/article/building-a-blockchain-in-net-core-transaction-and-reward/
    // https://www.c-sharpcorner.com/article/building-a-blockchain-in-net-core-p2p-network/
    // see also: https://www.jumpstartblockchain.com/article/learn-blockchain-in-c-sharp/

    // https://bitconseil.fr/blockchain-consensus-pow-pos-dpos
    // https://bitconseil.fr/proof-of-work-definition-explication/

    public class Blockchain
    {
        public readonly Transaction GenesisBlockTransaction = new Transaction(null, null, 0);

        public int Difficulty { get; set; } = 2;
        public IList<Block> Chain { get; set; }
        IList<Transaction> PendingTransactions = new List<Transaction>();
        public int Reward { get; set; } = 1;

        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }

        private void InitializeChain()
        {
            Chain = new List<Block>();
        }

        private void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }

        private Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null, new List<Transaction> { });
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();

            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Mine(Difficulty);
            
            Chain.Add(block);
        }

        public bool IsValid()
        {
            for(int i = 1; i< Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if(currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }
            }

            return true;
        }

        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public void ProcessPendingTransactions(string minerAddress)
        {
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);

            PendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction(null, minerAddress, Reward));
        }

        public int GetBalance(string address)
        {
            var allTransactions = this.Chain
                .SelectMany(block => block.Transactions);

            var credit = allTransactions
                .Where(transaction => address.Equals(transaction.ToAddress))
                .Sum(transaction => transaction.Amount);

            var debit = allTransactions
                .Where(transaction => address.Equals(transaction.FromAddress))
                .Sum(transaction => transaction.Amount);

            return credit - debit;
        }

    }
}
