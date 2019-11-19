using Blockchain.Core;
using System;
using System.Collections.Generic;
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
                new List<Transaction> { new Transaction("Bob", "Bill", 10) }));

            // Then
            Assert.Equal(2, blockchain.Chain.Count);
            Assert.Empty(blockchain.Chain[0].Transactions);
            Assert.Single(blockchain.Chain[1].Transactions);
            Assert.Equal("Bob", blockchain.Chain[1].Transactions[0].FromAddress);
            Assert.Equal("Bill", blockchain.Chain[1].Transactions[0].ToAddress);
            Assert.Equal(10, blockchain.Chain[1].Transactions[0].Amount);
            Assert.True(blockchain.IsValid());
        }

        [Fact]
        public void Is_Invalid_When_A_Block_Is_Modified()
        {
            // Given
            Core.Blockchain blockchain = new Core.Blockchain();

            Block latestBlock = blockchain.GetLatestBlock();

            blockchain.AddBlock(new Block(DateTime.Now, latestBlock.Hash,
                new List<Transaction> { new Transaction("Bob", "Bill", 10) }));

            // When
            blockchain.Chain[1].Transactions[0].Amount = 100;

            // Then
            Assert.False(blockchain.IsValid());
        }
    }
}
