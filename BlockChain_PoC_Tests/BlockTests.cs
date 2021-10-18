using BlockChain_PoC.Base;
using BlockChain_PoC.Interfaces;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace BlockChain_PoC_Tests
{
    public class BlockTests
    {
        private readonly System.Collections.Generic.List<ITransaction> _validTransactions;
        public BlockTests()
        {
            Random random = new Random();
            var transactionMoq = new Mock<ITransaction>();
            transactionMoq.Setup(x => x.IsValid()).Returns(true);
            _validTransactions = Enumerable.Repeat<ITransaction>(transactionMoq.Object, random.Next(1, 15)).ToList();
        }
        [Fact]
        public void MineBlock_ValidTransaction_Tests()
        {
            var block = new Block(this._validTransactions, 1, null, 3);
            block.MineBlock();
            var isValid = block.IsValid();
            Assert.True(isValid);
        }
    }
}