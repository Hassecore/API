using Hassecore.API.Data.Entities.PeriodTracking;

namespace Hassecore.API.Business.DTOs.PeriodTracking
{
    public class CreatePeriodEventDto
    {
        public required PeriodEventTypes EventType { get; set; }
        public required DateTime EventDate { get; set; }
    }
}
