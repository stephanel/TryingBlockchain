using System;
using System.Collections.Generic;

namespace Blockchain.Core
{
    // https://www.c-sharpcorner.com/article/blockchain-basics-building-a-blockchain-in-net-core/
    // https://www.c-sharpcorner.com/article/building-a-blockchain-in-net-core-proof-of-work/
    // https://www.c-sharpcorner.com/article/building-a-blockchain-in-net-core-transaction-and-reward/
    // https://www.c-sharpcorner.com/article/building-a-blockchain-in-net-core-p2p-network/
    // see also: https://www.jumpstartblockchain.com/article/learn-blockchain-in-c-sharp/

    public class Blockchain
    {
        public const string GenesisBlockData = "{}";

        public int Difficulty { get; set; } = 2;
        public IList<Block> Chain { get; set; }
         
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
            return new Block(DateTime.Now, null, GenesisBlockData);
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

    }
}
