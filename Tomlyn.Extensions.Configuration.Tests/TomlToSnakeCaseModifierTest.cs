using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Tomlyn.Extensions.Configuration.Tests;

public class TomlToSnakeCaseModifierTest
{
    [Fact]
    public void BindCorrect()
    {
        var sample = new SampleWithUnderscores();
        var configuration = new ConfigurationBuilder()
            .AddTomlFile("sample_with_underscores.toml")
            .Build();
        
        // ONLY NECESSARY if your variable binding DOES NOT have underscores and is PascalCase.
        // E.g., pascal_case in your .toml file is mapped to PascalCase in your binding class NOT Pascal_Case.
        var config = TomlToSnakeCaseModifier.RemoveUnderscores(configuration);
        config.Bind(sample);

        // The configuration system is type-agnostic and will not attempt to convert to numeric types
        // when the target is an object type given the DataValues variable declaration.
        foreach (var item in sample.DatabaseInformation.DataValues)
        {
            for (int i = 0; i < item.Count; i++)
            {
                if (double.TryParse(item[i].ToString(), out double result))
                {
                    item[i] = result;
                }
            }
        }

        var expected = new SampleWithUnderscores
        {
            Title = "TOML Example",
            OwnerOfToml = new OwnerOfToml
            {
                FullName = "Tom Preston-Werner",
                DateOfBirth = new DateTime(1979, 05, 27),
            },
            DatabaseInformation = new DatabaseInformation
            {
                IsEnabled = true,
                Ports = new ushort[] { 8000, 8001, 8002 },
                DataValues = new List<List<object>>
                {
                    new List<object> { "delta", "phi" },
                    new List<object> { 3.14 }
                },
                TempTargets = new Dictionary<string, decimal>
                {
                    ["cpu"] = 79.5m,
                    ["case"] = 72m,
                }
            },
            Servers = new Dictionary<string, Servers>
            {
                ["alpha"] = new()
                {
                    IpAddress = "10.0.0.1",
                    RoleInformation = RoleInformation.Frontend,
                },
                ["beta"] = new()
                {
                    IpAddress = "10.0.0.2",
                    RoleInformation = RoleInformation.Backend,
                },
            }
        };
        sample.Should().BeEquivalentTo(expected);
    }
}