using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sharkingbird.Jarvis.Core.Application;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Infrastructure;
using Sharkingbird.Jarvis.Infrastructure.Contracts;
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
      return collectionParam
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
