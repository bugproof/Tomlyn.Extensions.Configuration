using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Tomlyn.Extensions.Configuration.Tests;

public class TomlConfigurationExtensionsTest
{
    [Fact]
    public void Bind()
    {
        var sample = new Sample();
        var configuration = new ConfigurationBuilder().AddTomlFile("sample.toml").Build();
        configuration.Bind(sample);

        var expected = new Sample
        {
            Title = "TOML Example",
            Owner = new Owner
            {
                Name = "Tom Preston-Werner",
                DoB = new DateTime(1979, 05, 27),
            },
            Database = new Database
            {
                Enabled = true,
                Ports = new ushort[] { 8000, 8001, 8002 },
                Temp_Targets = new Dictionary<string, decimal>
                {
                    ["cpu"] = 79.5m,
                    ["case"] = 72m,
                }
            },
            Servers = new Dictionary<string, Server>
            {
                ["alpha"] = new()
                {
                    Ip = "10.0.0.1",
                    Role = Role.Frontend,
                },
                ["beta"] = new()
                {
                    Ip = "10.0.0.2",
                    Role = Role.Backend,
                },
            }
        };

        sample.Should().BeEquivalentTo(expected);
    }
}