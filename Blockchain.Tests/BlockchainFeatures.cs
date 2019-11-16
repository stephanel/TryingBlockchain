using Blockchain.Core;
using System;
using Xunit;

namespace Blockchain.Tests
{
    public class BlockchainFeatures
    {
        [Fact]
        public void Chaining_New_Block()
        {
            // Given
            Core.Blockchain blockchain = new Core.Blockchain();

            Block latestBlock = blockchain.GetLatestBlock();

            // When
            blockchain.AddBlock(new Block(DateTime.Now, latestBlock.Hash,
                "{sender:Bob,receiver:Bub,amount:10}"));

            // Then
            Assert.Equal(2, blockchain.Chain.Count);
            Assert.Equal(Core.Blockchain.GenesisBlockData, blockchain.Chain[0].Data);
            Assert.Equal("{sender:Bob,receiver:Bub,amount:10}", blockchain.Chain[1].Data);
            Assert.True(blockchain.IsValid());
        }

        [Fact]
        public void Is_Invalid_When_A_Block_Is_Modified()
        {
            // Given
            Core.Blockchain blockchain = new Core.Blockchain();

            Block latestBlock = blockchain.GetLatestBlock();

            blockchain.AddBlock(new Block(DateTime.Now, latestBlock.Hash,
                "{sender:Bob,receiver:Bub,amount:10}"));

            // When
            blockchain.Chain[1].Data = "{sender:Bob,receiver:Bub,amount:100}";

            // Then
            Assert.False(blockchain.IsValid());
        }
    }
}
