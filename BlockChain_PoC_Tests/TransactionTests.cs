using BlockChain_PoC.Base;
using BlockChain_PoC.Crypto;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlockChain_PoC_Tests
{
    public class TransactionTests
    {
        private readonly Faker _faker;
        public TransactionTests()
        { 
            _faker = new Faker();
        }
        [Xunit.Fact]
        public void MessageTransaction_ValidSignarute_Test()
        {
            var messageText = _faker.Lorem.Sentences(5);
            var key = KeyGen.CreateKeyPair();
            var transaction = new MessageTransaction(key.PublicKey, new byte[0], Encoding.UTF8.GetBytes(messageText));
            transaction.SignTransaction(key);
            var valid = transaction.IsValid();
            Assert.True(valid);
        }
    }
}
