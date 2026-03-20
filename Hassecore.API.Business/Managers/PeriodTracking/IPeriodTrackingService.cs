using Hassecore.API.Data.Entities.PeriodTracking;

namespace Hassecore.API.Business.Managers.PeriodTracking
{
    public interface IPeriodTrackingService
    {
        Task<bool> CreatePeriodEventAsync(PeriodEvent periodEvent);
        Task<List<PeriodEvent>> GetPeriodEventsAsync(Guid userId, Guid? pairedUserId);
        Task DeletePeriodEventAsync(Guid id);
    }
}
