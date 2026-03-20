using Hassecore.API.Business.Managers.PeriodTracking;
using Hassecore.API.Business.MediatR.PeriodTracking.Commands;
using Hassecore.API.Data.Context.CurrentUserContext;
using Hassecore.API.Data.Entities.PeriodTracking;
using MediatR;

namespace Hassecore.API.Business.MediatR.PeriodTracking.CommandHandlers
{
    public class CreatePeriodEventCommandHandler : IRequestHandler<CreatePeriodEventCommand, bool>
    {
        private readonly IPeriodTrackingService _periodTrackingService;
        private readonly CurrentUserContext _currentUserContext;

        public CreatePeriodEventCommandHandler(IPeriodTrackingService periodTrackingService,
                                               CurrentUserContext currentUserContext) 
        {
            _periodTrackingService = periodTrackingService;
            _currentUserContext = currentUserContext;
        }

        public async Task<bool> Handle(CreatePeriodEventCommand request, CancellationToken cancellationToken)
        {
            var periodEvent = new PeriodEvent
            {
                Id = Guid.NewGuid(),
                EventType = request.EventType,
                EventDate = request.EventDate,
                UserId = _currentUserContext.UserId
            };

            return await _periodTrackingService.CreatePeriodEventAsync(periodEvent);
        }
    }
}
