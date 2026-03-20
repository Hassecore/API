using Hassecore.API.Data.Entities.UserPairing;

namespace Hassecore.API.Business.Managers.UserPairing
{
    public interface IUserPairingService
    {
        //Task<bool> IsAnyOfUsersPaired(Guid userId1, Guid userId2);
        Task<bool> CreateUserPairingRequestAsync(Guid senderId, Guid receiverId);
        Task<bool> RevokeUserPairingRequestAsync(Guid revokingUserId);
        Task<bool> AcceptUserPairingRequestAsync(Guid acceptingUserId);
        Task<bool> DenyUserPairingRequestAsync(Guid denyingUserId);
        Task<UserPair?> GetUserPairAsync(Guid queryingUserId);
        Task<UserPairingRequest?> GetUserPairingRequestAsync(Guid queryingUserId);
    }
}
