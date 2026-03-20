using Hassecore.API.Data.Entities.UserPairing;
using Hassecore.API.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hassecore.API.Business.Managers.UserPairing
{
    public class UserPairingService : IUserPairingService
    {
        IBaseRepository _baseRepository;
        public UserPairingService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<bool> CreateUserPairingRequestAsync(Guid senderId, Guid receiverId)
        {
            var userPairingRequestExists = DoesUserPairingRequestExist(senderId, receiverId);
            if (IsAnyOfUsersPaired(senderId, receiverId) ||
                userPairingRequestExists)
            {
                return false;
            }

            var userPairingRequest = new UserPairingRequest
            {
                Id =  Guid.NewGuid(),
                SenderId = senderId,
                ReceiverId = receiverId,
                CreatedAt = DateTime.UtcNow
            };
            
            await _baseRepository.CreateAsync(userPairingRequest);

            return true;
        }
        
        public async Task<bool> RevokeUserPairingRequestAsync(Guid revokingUserId)
        {
            var userPairingRequest = await _baseRepository.GetSingleOrDefaultAsync<UserPairingRequest>(x => x.SenderId == revokingUserId);

            if (userPairingRequest == null)
            {
                return false;
            }

            await _baseRepository.DeleteAsync<UserPairingRequest>(userPairingRequest.Id);

            return true;
        }

        public async Task<bool> AcceptUserPairingRequestAsync(Guid acceptingUserId)
        {
            var pairingRequest = await _baseRepository.GetSingleOrDefaultAsync<UserPairingRequest>(x=> x.ReceiverId == acceptingUserId);
            if (pairingRequest == null)
            {
                return false;
            }

            if (IsAnyOfUsersPaired(pairingRequest.SenderId, pairingRequest.ReceiverId))
            {
                return false;
            }

            var userPair = new UserPair
            {
                Id = Guid.NewGuid(),
                User1Id = pairingRequest.SenderId,
                User2Id = pairingRequest.ReceiverId,
                CreatedAt = DateTime.UtcNow
            };
            await _baseRepository.CreateAsync(userPair);

            await _baseRepository.DeleteAsync<UserPairingRequest>(pairingRequest.Id);

            return true;
        }

        public async Task<bool> DenyUserPairingRequestAsync(Guid denyingUserId)
        {
            var pairingRequest = await _baseRepository.GetSingleOrDefaultAsync<UserPairingRequest>(x => x.ReceiverId == denyingUserId);
            if (pairingRequest == null)
            {
                return false;
            }
            await _baseRepository.DeleteAsync<UserPairingRequest>(pairingRequest.Id);
            return true;
        }

        public async Task<UserPair?> GetUserPairAsync(Guid queryingUserId)
        {
            var userPair = await _baseRepository.GetQueryable<UserPair>(x => x.User1Id == queryingUserId || x.User2Id == queryingUserId)
                                          .Include(x => x.User1)
                                          .Include(x => x.User2)
                                          .SingleOrDefaultAsync();

            return userPair;
        }

        public async Task<UserPairingRequest?> GetUserPairingRequestAsync(Guid queryingUserId)
        {
            var userPair = await _baseRepository.GetQueryable<UserPairingRequest>(x => x.SenderId == queryingUserId || x.ReceiverId == queryingUserId)
                                                .Include(x => x.Sender)
                                                .Include(x => x.Receiver)
                                                .SingleOrDefaultAsync();

            return userPair;
        }

        private bool DoesUserPairingRequestExist(Guid userId1, Guid userId2) => 
            _baseRepository.GetQueryable<UserPairingRequest>(x => x.SenderId == userId1 && x.ReceiverId == userId2 ||
                                                                  x.SenderId == userId2 && x.ReceiverId == userId1)
                           .Any();

        private bool IsAnyOfUsersPaired(Guid userId1, Guid userId2)
        {
            var anyUserPair = _baseRepository.GetQueryable<UserPair>(up => up.User1Id == userId1 || up.User2Id == userId1 ||
                                                                        up.User1Id == userId2 || up.User2Id == userId2)
                                          .Any();

            return anyUserPair;
        }
    }
}
