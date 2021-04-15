using Microsoft.Extensions.Configuration;

namespace Tomlyn.Extensions.Configuration
{
    public class TomlConfigurationSource : FileConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            FileProvider ??= builder.GetFileProvider();
            return new TomlConfigurationProvider(this);
        }
    }
}