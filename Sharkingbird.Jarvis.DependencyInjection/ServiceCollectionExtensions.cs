using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sharkingbird.Jarvis.Core.Application;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Infrastructure;
using Sharkingbird.Jarvis.Infrastructure.Configuration;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
using Sharkingbird.Jarvis.Infrastructure.Infrastructure;
using Sharkingbird.Jarvis.Infrastructure.RecurringPayments;
using System.Linq;

namespace Sharkingbird.Jarvis.DependencyInjection
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddJarvisInfrastructure(this IServiceCollection collectionParam)
    {
      foreach(var serviceType in typeof(PepcoRecurringPaymentRepository).Assembly.GetTypes()
        .Where(t=>!t.IsAbstract && typeof(IRecurringPaymentRepository).IsAssignableFrom(t)))
      {
        collectionParam.AddTransient(typeof(IRecurringPaymentRepository), serviceType);
      }
      var sqlOptions = collectionParam.BuildServiceProvider().GetRequiredService<IOptions<SqlConfiguration>>().Value;

      return collectionParam
        .AddTransient<IBudgetRepository, BudgetRepository>()
        .AddTransient<ITransactionRepository, TransactionRepository>()
        .AddTransient<IIntentRepository, LuisIntentRepository>()
        .AddHttpClient()
        .AddDbContext<JarvisDbContext>(o =>
        {
          o.UseSqlServer(sqlOptions.JarvisSqlConnectionString);
        })
        .AddSingleton<IEmailService, EmailService>()
        .AddTransient<INotificationService,TwilioNotificationService>();
    }
    public static IServiceCollection AddJarvisApplication(this IServiceCollection collectionParam)
    {
      return collectionParam
        .AddMediatR(typeof(ProcessRecurringPaymentsHandler).Assembly);
    }
  }
}
