using Hassecore.API.Data.Entities.PeriodTracking;
using Hassecore.API.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hassecore.API.Business.Managers.PeriodTracking
{
    public class PeriodTrackingService : IPeriodTrackingService
    {
        IBaseRepository _baseRepository;

        public PeriodTrackingService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> CreatePeriodEventAsync(PeriodEvent periodEvent)
        {
            var eventAlreadyExists = _baseRepository.GetQueryable<PeriodEvent>(pe => pe.EventDate == periodEvent.EventDate && pe.EventType == periodEvent.EventType).Any();
            if (eventAlreadyExists)
            {
                return false;
            }

            await _baseRepository.CreateAsync(periodEvent);

            return true;
        }

        public async Task<List<PeriodEvent>> GetPeriodEventsAsync(Guid userId, Guid? pairedUserId)
        {
            Expression<Func<PeriodEvent, bool>> predicate = x => x.UserId == userId;

            if (pairedUserId.HasValue)
            {
                predicate = x => x.UserId == userId || x.UserId == pairedUserId;
            }

            return await _baseRepository.GetQueryable(predicate).ToListAsync();
        }

        public async Task DeletePeriodEventAsync(Guid id)
        {
            await _baseRepository.DeleteAsync<PeriodEvent>(id);
        }
    }
}
