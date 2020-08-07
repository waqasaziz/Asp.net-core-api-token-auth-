using Domain.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.Helpers
{
    public class TestHashingProvider
    {
        [Fact]
        public void Does_Hash_Match_The_Text()
        {
            // Arrange  
            var message = "passw0rd";

            var provider = CreateProvider();
            var salt = provider.GenerateSalt();
            var hash = provider.GenerateHash(message, salt);

            // Act  
            var match = provider.Validate(message, salt, hash);

            // Assert  
            Assert.True(match);
        }

        [Fact]
        public void Tampered_Hash_Should_Not_Match()
        {
            // Arrange  
            var message = "passw0rd";
            var hash = "GwTRDTrQSoeCxVTFr6dtYly7d0cPxIak=";

            var provider = CreateProvider();
            var salt = provider.GenerateSalt();


            // Act  
            var match = provider.Validate(message, salt, hash);

            // Assert  
            Assert.False(match);
        }

        [Fact]
        public void Different_Hashes_Should_Not_Match()
        {
            // Arrange  
            var message1 = "passw0rd";
            var message2 = "password";
            var provider = CreateProvider();
            var salt = provider.GenerateSalt();

            // Act  
            var hash1 = provider.GenerateHash(message1, salt);
            var hash2 = provider.GenerateHash(message2, salt);

            // Assert  
            Assert.NotEqual(hash1, hash2);
        }

        private IHashingProvider CreateProvider() => new HashingProvider(Options.Create(new HashingOptions()));
    }
}
