using Hassecore.API.Business.Managers.PeriodTracking;
using Hassecore.API.Business.MediatR.PeriodTracking.Commands;
using MediatR;

namespace Hassecore.API.Business.MediatR.PeriodTracking.CommandHandlers
{
    public class DeletePeriodEventCommandHandler : IRequestHandler<DeletePeriodEventCommand>
    {
        private readonly IPeriodTrackingService _periodTrackingService;
        public DeletePeriodEventCommandHandler(IPeriodTrackingService periodTrackingService)
        {
            _periodTrackingService = periodTrackingService;
        }

        public async Task Handle(DeletePeriodEventCommand request, CancellationToken cancellationToken)
        {
            await _periodTrackingService.DeletePeriodEventAsync(request.PeriodEventId);
        }
    }
}
