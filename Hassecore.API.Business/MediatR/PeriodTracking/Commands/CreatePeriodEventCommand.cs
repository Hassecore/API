using Hassecore.API.Data.Entities.PeriodTracking;
using MediatR;

namespace Hassecore.API.Business.MediatR.PeriodTracking.Commands
{
    public class CreatePeriodEventCommand : IRequest<bool>
    {
        public required PeriodEventTypes EventType { get; set; }
        public required DateTime EventDate { get; set; }
    }
}
