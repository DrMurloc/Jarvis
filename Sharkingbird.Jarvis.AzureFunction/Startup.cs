using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sharkingbird.Jarvis.AzureFunction;
using Sharkingbird.Jarvis.DependencyInjection;
using Sharkingbird.Jarvis.Infrastructure.Configuration;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class Startup : FunctionsStartup
  {
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
      builder.ConfigurationBuilder
        .AddJsonFile("appsettings.json",true)
        .AddEnvironmentVariables()
        .AddUserSecrets<Startup>(true);
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
      var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
      builder.Services
        .AddOptions<EmailConfiguration>()
        .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("Email").Bind(settings); })
        .Services
        .AddOptions<TwilioConfiguration>()
        .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("Twilio").Bind(settings); })
        .Services
        .AddOptions<SqlConfiguration>()
        .Configure<IConfiguration>((settings, configuration) =>
        {
          settings.JarvisSqlConnectionString = configuration.GetConnectionString("JarvisSql");
        })
        .Services
        .AddOptions<LuisConfiguration>()
        .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("Luis").Bind(settings); })
        .Services
        .AddOptions<GoogleConfiguration>()
        .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("Google").Bind(settings); })
        .Services
        .AddOptions<AccountConfiguration>()
        .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("Account").Bind(settings); })
        .Services
        .AddJarvisInfrastructure()
        .AddJarvisApplication();
    }
  }
}
