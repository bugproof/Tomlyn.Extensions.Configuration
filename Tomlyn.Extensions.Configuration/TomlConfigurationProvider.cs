using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Tomlyn.Extensions.Configuration
{
    public class TomlConfigurationProvider : FileConfigurationProvider
    {
        public TomlConfigurationProvider(FileConfigurationSource source) : base(source)
        {
        }

        public override void Load(Stream stream)
        {
            try
            {
                Data = TomlConfigurationFileParser.Parse(stream);
            }
            catch (Exception e)
            {
                throw new FormatException("TOML parse failed", e);
            }
        }
    }
}