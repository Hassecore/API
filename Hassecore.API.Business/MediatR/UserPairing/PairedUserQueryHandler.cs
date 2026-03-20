using Hassecore.API.Business.DTOs.UserPairing;
using Hassecore.API.Business.Managers.UserPairing;
using MediatR;

namespace Hassecore.API.Business.MediatR.UserPairing
{
    internal class PairedUserQueryHandler : IRequestHandler<PairedUserQuery, PairedUserResponseDto?>
    {
        private readonly IUserPairingService _userPairingService;
        public PairedUserQueryHandler(IUserPairingService userPairingService)
        {
                _userPairingService = userPairingService;
        }

        public async Task<PairedUserResponseDto?> Handle(PairedUserQuery request, CancellationToken cancellationToken)
        {
            var userPair = await _userPairingService.GetUserPairAsync(request.QueryingUserId);

            if (userPair == null)
            {
                return null;
            }

            if (userPair.User1Id == request.QueryingUserId)
            {
                return new PairedUserResponseDto
                {
                    UserId = userPair.User2Id,
                    UserName = userPair.User2!.Username,
                    Email = userPair.User2.Email,
                };
            }
            else
            {
                return new PairedUserResponseDto
                {
                    UserId = userPair.User1Id,
                    UserName = userPair.User1!.Username,
                    Email = userPair.User1.Email,
                };
            }
        }
    }
}
