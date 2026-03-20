using Hassecore.API.Business.DTOs.PeriodTracking;
using Hassecore.API.Business.Managers.PeriodTracking;
using Hassecore.API.Business.MediatR.PeriodTracking.Queries;
using Hassecore.API.Data.Context.CurrentUserContext;
using MediatR;

namespace Hassecore.API.Business.MediatR.PeriodTracking.QueryHandlers
{
    public class GetPeriodEventsQueryHandler : IRequestHandler<GetPeriodEventsQuery, List<PeriodEventReturnDto>>
    {
        private readonly IPeriodTrackingService _periodTrackingService;
        private readonly CurrentUserContext _currentUserContext;

        public GetPeriodEventsQueryHandler(IPeriodTrackingService periodTrackingService,
                                           CurrentUserContext currentUserContext)
        {
            _periodTrackingService = periodTrackingService;
            _currentUserContext = currentUserContext;
        }

        public async Task<List<PeriodEventReturnDto>> Handle(GetPeriodEventsQuery request, CancellationToken cancellationToken)
        {
            var result = await _periodTrackingService.GetPeriodEventsAsync(_currentUserContext.UserId, _currentUserContext.PairedUserId);

            var periodEventsForUser = result.OrderByDescending(x => x.EventDate)
                                            .Select(x => new PeriodEventReturnDto
                                                        {
                                                            Id = x.Id,
                                                            EventType = x.EventType,
                                                            //EventDate = x.EventDate,
                                                            EventDate = x.EventDate.ToString("MMM dd, yy")
                                                        })
                                            .ToList();

            return periodEventsForUser;
        }
    }
}
