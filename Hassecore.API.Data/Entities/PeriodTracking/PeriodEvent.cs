using Hassecore.API.Data.Entities.UserPairing;

namespace Hassecore.API.Data.Entities.PeriodTracking
{
    public class PeriodEvent : IEntityBase
    {
        public required Guid Id { get; set; }
        public required PeriodEventTypes EventType { get; set; }
        public required DateTime EventDate { get; set; }
        public required Guid UserId { get; set; }
        public User? User { get; set; }

    }
}
