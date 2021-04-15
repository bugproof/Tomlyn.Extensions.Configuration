# Tomlyn.Extensions.Configuration [![NuGet](https://img.shields.io/nuget/v/Tomlyn.Extensions.Configuration)](https://www.nuget.org/packages/Tomlyn.Extensions.Configuration/)
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
or
```toml
AllowedHosts = "*"

[Logging.LogLevel]
Default = "Information"
Microsoft = "Warning"
"Microsoft.Hosting.Lifetime" = "Information"
```

Then you can access it like `Configuration["Logging:LogLevel:Default"]`. Binding with [`IOptions<TOptions>`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0) should work too.
