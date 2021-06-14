﻿using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sharkingbird.Jarvis.AzureFunction;
using Sharkingbird.Jarvis.DependencyInjection;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Sharkingbird.Jarvis.AzureFunction
{
  public sealed class Startup : FunctionsStartup
  {
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
      builder.ConfigurationBuilder.AddUserSecrets<Startup>(true);
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
      var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
      builder.Services.AddJarvisInfrastructure()
        .AddJarvisApplication()
        .AddOptions<EmailConfiguration>()
        .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("Email").Bind(settings); })
        .Services
        .AddOptions<TwilioConfiguration>()
        .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("Twilio").Bind(settings); });
    }
  }
}