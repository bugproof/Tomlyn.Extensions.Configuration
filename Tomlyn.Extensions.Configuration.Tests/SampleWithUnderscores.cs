using System;
using System.Collections.Generic;

namespace Tomlyn.Extensions.Configuration.Tests;

public class SampleWithUnderscores
{
    public string Title { get; init; } = "";
    public OwnerOfToml OwnerOfToml { get; init; } = new();
    public DatabaseInformation DatabaseInformation { get; init; } = new();
    public Dictionary<string, Servers> Servers { get; init; } = new();

}

public class OwnerOfToml
{
    public string FullName { get; init; } = "";
    public DateTime DateOfBirth { get; init; }
}

public class DatabaseInformation
{
    public bool IsEnabled { get; init; }
    public ushort[] Ports { get; init; } = Array.Empty<ushort>();
    public List<List<object>> DataValues { get; init; } = new();

    public Dictionary<string, decimal> TempTargets { get; init; } = new();
}

public class Servers
{
    public string IpAddress { get; init; } = "";
    public RoleInformation RoleInformation { get; init; }
}

public enum RoleInformation
{
    Unknown,
    Frontend,
    Backend,
}
