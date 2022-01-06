using System.IO;
using Microsoft.Extensions.Configuration;

namespace Tomlyn.Extensions.Configuration
{
    public class TomlStreamConfigurationProvider : StreamConfigurationProvider
    {
        public TomlStreamConfigurationProvider(TomlStreamConfigurationSource source) : base(source) { }
    
        public override void Load(Stream stream)
        {
            Data = TomlConfigurationFileParser.Parse(stream);
        }
    }
}