﻿using System;
using Microsoft.Extensions.Configuration;

namespace Domain.Helpers
{


    public class ConfigFileKeyProvider : IEncryptionKeyProvider
    {
        private readonly IConfiguration _config;

        public ConfigFileKeyProvider(IConfiguration config)
        {
            _config = config;
        }

        public string PublicKey => _config["RSAKeys:Public"];

        public string PrivateKey => _config["RSAKeys:Private"];
    }
}
