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
using Sharkingbird.Jarvis.Infrastructure.Repositories;
using Sharkingbird.Jarvis.Infrastructure.Services;
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
      var cosmosOptions = collectionParam.BuildServiceProvider().GetRequiredService<IOptions<CosmosConfiguration>>().Value;

      return collectionParam
        .AddTransient<IBudgetService,BudgetService>()
        .AddTransient<IBudgetRepository,SqlBudgetRepository>()
        .AddTransient<IIntentRepository, LuisIntentRepository>()
        .AddTransient<IVacationRepository, GoogleCalendarVacationRepository>()
        .AddTransient<ITransactionRepository,SqlTransactionRepository>()
        .AddTransient<ITransactionNotificationsRepository,PncEmailTransactionNotificationRepository>()
        .AddHttpClient()
        .AddDbContext<JarvisDbContext>(o =>
        {
          o.UseCosmos(cosmosOptions.AccountEndpoint,cosmosOptions.AccountKey,cosmosOptions.DatabaseName);
        })
        .AddSingleton<IEmailService, EmailService>()
        .AddTransient<INotificationService,TwilioNotificationService>();
    }
    public static IServiceCollection AddJarvisApplication(this IServiceCollection collectionParam)
    {
      return collectionParam
        .AddMediatR(typeof(ProcessRecurringPaymentsHandler).Assembly, typeof(TwilioNotificationService).Assembly);
    }
  }
}
