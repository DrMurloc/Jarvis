using MediatR;
using Sharkingbird.Jarvis.Core.Contracts;
using Sharkingbird.Jarvis.Core.Mediation.Commands;
using Sharkingbird.Jarvis.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharkingbird.Jarvis.Core.Application
{
  public sealed class ImportVacationsHandler : IRequestHandler<ImportVacationsCommand>
  {
    private readonly IVacationRepository _vacationRepository;
    private readonly ICalendarEventRepository _calendarEventRepository;
    private readonly INotificationService _notificationService;
    public ImportVacationsHandler(IVacationRepository vacationRepository,
      ICalendarEventRepository calendarEventRepository,
      INotificationService notificationService)
    {
      _vacationRepository = vacationRepository;
      _calendarEventRepository = calendarEventRepository;
      _notificationService = notificationService;
    }
    public async Task<Unit> Handle(ImportVacationsCommand request, CancellationToken cancellationToken)
    {
      var calendarEvents = await _calendarEventRepository.GetUpcomingCalanderEvents(cancellationToken);
      var existingVacations = (await _vacationRepository.GetVacations(cancellationToken)).Select(v => v.Id).ToHashSet();

      var missingEvents = calendarEvents.Where(c => !existingVacations.Contains(c.Id))
        .ToArray();
      if (!missingEvents.Any())
      {
        return Unit.Value;
      }
      await _vacationRepository.SaveVacations(missingEvents,cancellationToken);
      var message = $"Vacations imported: {string.Join(", ", missingEvents.Select(v => v.ToString()))}";
      await _notificationService.PushNotification(new Notification(message), cancellationToken);
      return Unit.Value;
    }
  }
}
