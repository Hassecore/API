using Hassecore.API.Data.Entities.PeriodTracking;

namespace Hassecore.API.Business.DTOs.PeriodTracking
{
    public class PeriodEventReturnDto
    {
        public required Guid Id { get; set; }
        public required PeriodEventTypes EventType { get; set; }
        //public required DateTime EventDate { get; set; }
        public required string EventDate { get; set; }
    }
}
