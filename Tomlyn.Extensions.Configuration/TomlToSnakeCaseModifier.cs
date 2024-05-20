using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Tomlyn.Extensions.Configuration;

/// <summary>
/// This class is used to allow the seamless binding between a .toml file that has snake_case keys (key_one = value)
/// and pascal variables that are being bound (string KeyOne = "value"). The implementation stays relatively the same,
/// BUT you need to use 'RemoveUnderscores' after you use '.Build()' when building the configuration, and before
/// using '.Bind()' when you attempt to bind the configuration to some class that holds your pascal variables.
///
/// This can be achieved by using this line of code:
/// 'var config = TomlToSnakeCaseModifier.RemoveUnderscores(configuration);'.
/// </summary>
public static class TomlToSnakeCaseModifier
{
    /// <summary>
    /// This method creates a ConfigurationBuilder object that is modeled after the
    /// configuration of the .toml file that is passed. It modifies the configuration keys
    /// by removing underscores.
    /// </summary>
    /// <param name="configuration">Represents the configuration from a .toml file that has underscores.</param>
    /// <returns>A new IConfiguration object with modified keys that have no underscores.</returns>
    public static IConfiguration RemoveUnderscores(this IConfiguration configuration)
    {
        var newConfig = new ConfigurationBuilder();
        AddStrippedKeys(configuration, newConfig, "");
        return newConfig.Build();
    }

    /// <summary>
    /// The method is designed to recursively iterate through a configuration structure,
    /// modify its keys by removing underscores, and then add these modified key-value pairs
    /// to a new IConfigurationBuilder.
    /// </summary>
    /// <param name="configuration">The IConfiguration representing the .toml file (has underscores).</param>
    /// <param name="newConfig">The IConfiguration that is modified with keys that have no underscores.</param>
    /// <param name="currentPath">Current path of the configuration key, separated by colons for nesting.</param>
    private static void AddStrippedKeys(IConfiguration configuration, IConfigurationBuilder newConfig, string currentPath)
    {
        // Iterate through each key in the current configuration section
        foreach (var child in configuration.GetChildren())
        {
            var strippedKey = child.Key.Replace("_", "");
            var newPath = string.IsNullOrEmpty(currentPath) ? strippedKey : $"{currentPath}:{strippedKey}";

            // If the current child has further children, recurse into them
            if (child.GetChildren().Any())
                AddStrippedKeys(child, newConfig, newPath);
            else
                // No more children, add the current configuration item to the builder
                newConfig.AddInMemoryCollection(new Dictionary<string, string> { { newPath, child.Value } });
        }
    }
}