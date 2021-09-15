# Tomlyn.Extensions.Configuration [![NuGet](https://img.shields.io/nuget/v/Tomlyn.Extensions.Configuration)](https://www.nuget.org/packages/Tomlyn.Extensions.Configuration/)
TomlConfigurationProvider using [Tomlyn](https://github.com/xoofx/Tomlyn)

Mostly based on [Microsoft.Extensions.Configuration.Json](https://github.com/dotnet/runtime/tree/main/src/libraries/Microsoft.Extensions.Configuration.Json/src)

## Usage

```cs
// ASP.NET Core
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.Sources.Clear(); // CreateDefaultBuilder adds default configuration sources like appsettings.json. Here we can remove them

            var env = hostingContext.HostingEnvironment;
            
            bool reloadOnChange = hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", defaultValue: true);

            config.AddTomlFile("appsettings.toml", optional: true, reloadOnChange: reloadOnChange)
                .AddTomlFile($"appsettings.{env.EnvironmentName}.toml", optional: true, reloadOnChange: reloadOnChange);
                
            if (env.IsDevelopment() && env.ApplicationName is { Length: > 0 })
            {
                var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                if (appAssembly is not null)
                {
                    config.AddUserSecrets(appAssembly, optional: true, reloadOnChange: reloadOnChange);
                }
            }
            
            config.AddEnvironmentVariables();

            if (args is { Length: > 0 })
            {
                config.AddCommandLine(args);
            }
        })
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
// Console
var config = new ConfigurationBuilder()
    .AddTomlFile("appsettings.toml", optional: true, reloadOnChange: true)
    .Build();
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

Then you can access it like `Configuration["Logging:LogLevel:Default"]`. 

Binding with [`IOptions<TOptions>`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0) should work too.

## Case sensitivity

TOML is case-sensitive but [configuration keys aren't](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0#configuration-keys-and-values). So even though this is accepted when using Tomlyn alone, it's not accepted when using this library and exception will be thrown:

```toml
name = "Gina"
Name = "Gina"
```

## Alternatives and benchmarks

Another [possibly slightly faster](https://github.com/bugproof/TomlLibrariesBenchmark) alternative is [Tommy.Extensions.Configuration](https://github.com/dezhidki/Tommy/tree/master/Tommy.Extensions.Configuration)
