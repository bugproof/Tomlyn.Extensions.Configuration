# Tomlyn.Extensions.Configuration
TomlConfigurationProvider using [Tomlyn](https://github.com/xoofx/Tomlyn) because all the other TOML libs are dead. Mostly based on [Microsoft.Extensions.Configuration.Json](https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.Configuration.Json/src)

## Usage

```cs
var env = hostingContext.HostingEnvironment;

config
  .AddTomlFile("appsettings.toml", optional: true, reloadOnChange: true)
  .AddTomlFile($"appsettings.{env.EnvironmentName}.toml", optional: true, reloadOnChange: true);
```

## appsettings.toml

```toml
AllowedHosts = "*"

[Logging]

    [Logging.LogLevel]
        Default = "Information"
        Microsoft = "Warning"
        "Microsoft.Hosting.Lifetime" = "Information"
```
