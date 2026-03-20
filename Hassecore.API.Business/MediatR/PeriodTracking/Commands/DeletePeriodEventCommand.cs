using MediatR;

namespace Hassecore.API.Business.MediatR.PeriodTracking.Commands
{
    public class DeletePeriodEventCommand : IRequest
    {
        public required Guid PeriodEventId { get; set; }
    }
}
