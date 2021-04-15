# Tomlyn.Extensions.Configuration
TomlConfigurationProvider using Tomlyn

## Usage

```cs
var env = hostingContext.HostingEnvironment;

config
  .AddTomlFile("appsettings.toml", optional: true, reloadOnChange: true)
  .AddTomlFile($"appsettings.{env.EnvironmentName}.toml", optional: true, reloadOnChange: true);
```
