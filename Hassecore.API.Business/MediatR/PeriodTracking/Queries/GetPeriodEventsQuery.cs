using Hassecore.API.Business.DTOs.PeriodTracking;
using MediatR;

namespace Hassecore.API.Business.MediatR.PeriodTracking.Queries
{
    public class GetPeriodEventsQuery : IRequest<List<PeriodEventReturnDto>>
    {
    }
}
