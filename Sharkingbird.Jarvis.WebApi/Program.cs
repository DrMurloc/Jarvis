using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sharkingbird.Jarvis.DependencyInjection;
using Sharkingbird.Jarvis.Infrastructure.Configuration;

namespace Sharkingbird.Jarvis.WebApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
      .ConfigureAppConfiguration(builder =>
      {
        builder.AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>();
      })
      .ConfigureServices((context, services) =>
      {
        services.Configure<EmailConfiguration>(context.Configuration.GetSection("Email"))
          .Configure<TwilioConfiguration>(context.Configuration.GetSection("Twilio"))
          .AddJarvisInfrastructure()
          .AddJarvisApplication();
      })
      .ConfigureWebHostDefaults(webBuilder =>
      {
        webBuilder.UseStartup<Startup>();
      });
  }
}
