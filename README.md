# Tomlyn.Extensions.Configuration
TomlConfigurationProvider using [Tomlyn](https://github.com/xoofx/Tomlyn) because all the other TOML libs are dead

## Usage

```cs
var env = hostingContext.HostingEnvironment;

config
  .AddTomlFile("appsettings.toml", optional: true, reloadOnChange: true)
  .AddTomlFile($"appsettings.{env.EnvironmentName}.toml", optional: true, reloadOnChange: true);
```
